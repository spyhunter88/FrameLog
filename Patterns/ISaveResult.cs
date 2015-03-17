
namespace FrameLog.Patterns
{
    public interface ISaveResult<TChangeSet>
    {
        /// <summary>
        /// The number of objects in an System.Data.Entity.EntityState.Added, System.Data.Entity.EntityState.Modified,
        /// or System.Data.Entity.EntityState.Deleted state when SaveChanges was called, not including objects created
        /// by FrameLogModule for recording the change.
        /// </summary>
        int AffectedObjectCount { get; }

        /// <summary>
        /// The ChangeSet created by FrameLogModule to record changes, or null if there were no changes that were
        /// logged according to the filter rules of the FrameLogModule.
        /// </summary>
        TChangeSet ChangeSet { get; }
    }
}
