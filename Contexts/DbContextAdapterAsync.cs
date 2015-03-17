using System;
using System.Threading;
using System.Threading.Tasks;
using FrameLog.Models;

namespace FrameLog.Contexts
{
    public abstract partial class DbContextAdapter<TChangeSet, TPrincipal>
        where TChangeSet : IChangeSet<TPrincipal>
    {
        public override Task<int> SaveAndAcceptAllChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return context.SaveChangesAsync(cancellationToken);
        }
    }
}
