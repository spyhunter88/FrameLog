using FrameLog.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FrameLog.Contexts
{
    public partial interface IFrameLogContext<TChangeSet, TPrincipal>
        where TChangeSet : IChangeSet<TPrincipal>
    {
        Task<int> SaveAndAcceptAllChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
