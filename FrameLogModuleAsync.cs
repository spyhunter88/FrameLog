using System;
using FrameLog.Models;
using FrameLog.Transactions;
using System.Data.Entity.Core.Objects;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FrameLog.Logging;

namespace FrameLog
{
    public partial class FrameLogModule<TChangeSet, TPrincipal>
        where TChangeSet : IChangeSet<TPrincipal>
    {
        /// <summary>
        /// Save the changes and log them as controlled by the logging filter. 
        /// A TransactionScope will be used to wrap the save, which will use an ambient 
        /// transaction if available, or create a new one otherwise.
        ///  
        /// If you are using an explicit transaction, and not using the TransactionScope
        /// API, then do not use this overload. Use SaveChangesWithinExplicitTransaction.
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        public Task<ISaveResult<TChangeSet>> SaveChangesAsync(TPrincipal principal, CancellationToken cancellationToken = default(CancellationToken))
        {
            return SaveChangesAsync(principal, new TransactionOptions(), cancellationToken);
        }
        
        /// <summary>
        /// Save the changes and log them as controlled by the logging filter. 
        /// A TransactionScope will be used to wrap the save, which will use an ambient 
        /// transaction available, or create a new one otherwise. The given options
        /// will be used in constructing the TransactionScope.
        ///  
        /// If you are using an explicit transaction, and not using the TransactionScope
        /// API, then do not use this overload. Use SaveChangesWithinExplicitTransaction.
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        public Task<ISaveResult<TChangeSet>> SaveChangesAsync(TPrincipal principal, TransactionOptions transactionOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            return saveChangesAsync(principal, new TransactionScopeProvider(transactionOptions), cancellationToken);
        }

        /// <summary>
        /// Save the changes and log them as controlled by the logging filter. 
        /// Warning: Only use this overload when you are wrapping the call to FrameLog in your 
        /// own transaction. This prevents FrameLog from automatically creating its own transaction.
        /// 
        /// If you are using the TransactionScope API, you can use the SaveChanges overload,
        /// as FrameLog will automatically detect the ambient transaction.
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        public Task<ISaveResult<TChangeSet>> SaveChangesWithinExplicitTransactionAsync(TPrincipal principal, CancellationToken cancellationToken = default(CancellationToken))
        {
            // If there is already an explicit transaction in use, we don't need to do anything
            // with transactions in FrameLog, so just use the NullTransactionProvider
            return saveChangesAsync(principal, new NullTransactionProvider(), cancellationToken);
        }

        protected async Task<ISaveResult<TChangeSet>> saveChangesAsync(TPrincipal principal, ITransactionProvider transactionProvider, CancellationToken cancellationToken)
        {
            if (!Enabled)
                return new SaveResult<TChangeSet, TPrincipal>(await context.SaveAndAcceptAllChangesAsync(cancellationToken));

            var result = new SaveResult<TChangeSet, TPrincipal>();
            // We want to split saving and logging into two steps, so that when we
            // generate the log objects the database has already assigned IDs to new
            // objects. Then we can log about them meaningfully. So we wrap it in a
            // transaction so that even though there are two saves, the change is still
            // atomic.
            await transactionProvider.InTransactionAsync(async () =>
            {
                var logger = new ChangeLogger<TChangeSet, TPrincipal>(context, factory, filter, serializer);
                cancellationToken.ThrowIfCancellationRequested();

                // First we detect all the changes, but we do not save or accept the changes 
                // (i.e. we keep our record of them).
                context.DetectChanges();
                cancellationToken.ThrowIfCancellationRequested();

                // This returns the log objects, but they are not attached to the context
                // so the context change tracker won't noticed them
                var oven = logger.Log(context.ObjectStateManager);
                cancellationToken.ThrowIfCancellationRequested();

                // Then we save and accept the changes, but only the original changes - 
                // the context hasn't yet detected the log changes
                result.AffectedObjectCount = await context.SaveAndAcceptAllChangesAsync(cancellationToken);

                // NOTE: From this point in, we stop honoring the cancellation token.
                //       Why? because if we did, you could end up object changes commited without any logging.
                //       In the interest of data integrity, we either save the object changes + logging, or save nothing at all.
            
                // This code then attaches the log objects to the context
                if (oven.HasChangeSet)
                {
                    // First do any deferred log value calculations.
                    // (see PropertyChange.Bake for more information)
                    // Then detect all the log changes that were previously deferred
                    result.ChangeSet = oven.Bake(DateTime.Now, principal);
                    context.AddChangeSet(result.ChangeSet);
                    context.DetectChanges();

                    // Then we save and accept the changes that result from creating the log objects
                    context.SaveAndAcceptAllChanges();
                }
            });
            return result;
        }
    }
}
