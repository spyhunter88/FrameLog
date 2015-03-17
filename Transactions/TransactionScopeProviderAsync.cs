using FrameLog.Exceptions;
using System;
using System.Data.Entity.Core;
using System.Threading.Tasks;
using System.Transactions;

namespace FrameLog.Transactions
{
    /// <summary>
    /// Wraps the given operations in a TransactionScope-based transaction
    /// </summary>
    public partial class TransactionScopeProvider
    {
        public async Task InTransactionAsync(Func<Task> taskAction)
        {
            // Short circuit
            if (taskAction == null)
                return;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, options, TransactionScopeAsyncFlowOption.Enabled))
                {
                    await taskAction();
                    scope.Complete();
                }
            }
            catch (EntityException e)
            {
                if (ConflictingTransactionException.Matches(e))
                    throw new ConflictingTransactionException(e);

                throw;
            }
        }
    }
}
