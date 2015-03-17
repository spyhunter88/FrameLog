using FrameLog.Exceptions;
using System;
using System.Data.Entity.Core;
using System.Transactions;

namespace FrameLog.Transactions
{
    /// <summary>
    /// Wraps the given operations in a TransactionScope-based transaction
    /// </summary>
    public partial class TransactionScopeProvider : ITransactionProvider
    {
        private readonly TransactionOptions options;

        public TransactionScopeProvider(TransactionOptions options)
        {
            this.options = options;
        }

        public void InTransaction(Action action)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    action();
                    scope.Complete();
                }
            }
            catch (EntityException e)
            {
                if (ConflictingTransactionException.Matches(e))
                    throw new ConflictingTransactionException(e);
                else
                    throw;
            }
        }
    }
}
