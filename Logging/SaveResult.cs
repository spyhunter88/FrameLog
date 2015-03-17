using FrameLog.Patterns;
using FrameLog.Patterns.Models;

namespace FrameLog.Logging
{
    internal class SaveResult<TChangeSet, TPrincipal> : ISaveResult<TChangeSet>
        where TChangeSet : IChangeSet<TPrincipal>
    {
        internal SaveResult()
        {
            AffectedObjectCount = 0;
            ChangeSet = default(TChangeSet);
        }
        internal SaveResult(int affectedObjectCount)
            : this()
        {
            AffectedObjectCount = affectedObjectCount;
        }

        internal int AffectedObjectCount { get; set; }
        internal TChangeSet ChangeSet { get; set; }

        int ISaveResult<TChangeSet>.AffectedObjectCount { get { return AffectedObjectCount; } }
        TChangeSet ISaveResult<TChangeSet>.ChangeSet { get { return ChangeSet; } }
    }
}
