using System;

namespace FrameLog.Transactions
{
    /// <summary>
    /// Wraps the given action in some kind of transaction, or no transaction,
    /// based on the implementation. This is so that different transaction
    /// systems can be used as appropriate.
    /// </summary>
    public partial interface ITransactionProvider
    {
        void InTransaction(Action action);
    }
}
