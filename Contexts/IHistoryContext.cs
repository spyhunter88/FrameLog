using FrameLog.Models;
using System.Linq;
using FrameLog.Patterns.Models;

namespace FrameLog.Contexts
{
    public interface IHistoryContext
    {
        /// <summary>
        /// Returns true if the object has a logging reference, otherwise false.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool ObjectHasReference(object model);
        /// <summary>
        /// Returns a unique reference that FrameLog uses to refer to the object
        /// in the logs. Normally the primary key.
        /// </summary>
        string GetReferenceForObject(object model);
        /// <summary>
        /// Returns the primary key property for an object
        /// </summary>
        string GetReferencePropertyForObject(object model);
        /// <summary>
        /// Returns the object of the specified type that has the specified reference.
        /// GetReferenceForObject(GetObjectByReference(type, reference)) == reference
        /// </summary>
        object GetObjectByReference(System.Type type, string raw);
    }

    public interface IHistoryContext<TChangeSet, TPrincipal> : IHistoryContext
        where TChangeSet : IChangeSet<TPrincipal>
    {
        IQueryable<IChangeSet<TPrincipal>> ChangeSets { get; }
        IQueryable<IObjectChange<TPrincipal>> ObjectChanges { get; }
        IQueryable<IPropertyChange<TPrincipal>> PropertyChanges { get; }
    }
}
