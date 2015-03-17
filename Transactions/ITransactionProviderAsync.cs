using System;
using System.Threading.Tasks;

namespace FrameLog.Transactions
{
    /// <summary>
    /// Wraps the given action in some kind of transaction, or no transaction,
    /// based on the implementation. This is so that different transaction
    /// systems can be used as appropriate.
    /// </summary>
    public partial interface ITransactionProvider
    {
        Task InTransactionAsync(Func<Task> taskAction);
    }
}
