using System;
using System.Threading.Tasks;

namespace FrameLog.Transactions
{
    /// <summary>
    /// Does not wrap the action in a transaction - just invokes it.
    /// This is for when there is an existing transaction already open,
    /// and FrameLog doesn't need to do anything to be covered by it.
    /// </summary>
    public partial class NullTransactionProvider
    {
        public async Task InTransactionAsync(Func<Task> taskAction)
        {
            if (taskAction != null)
                await taskAction();
        }
    }
}
