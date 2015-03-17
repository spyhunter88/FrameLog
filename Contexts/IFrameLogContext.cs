using FrameLog.Models;
using System;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using FrameLog.Patterns.Models;

namespace FrameLog.Contexts
{
    public interface ContextInfo
    {
        Type UnderlyingContextType { get; }
        MetadataWorkspace Workspace { get; }
    }

    public partial interface IFrameLogContext<TChangeSet, TPrincipal> : IHistoryContext<TChangeSet, TPrincipal>, ContextInfo
        where TChangeSet : IChangeSet<TPrincipal>
    {
        ObjectStateManager ObjectStateManager { get; }
        void DetectChanges();
        int SaveAndAcceptAllChanges();

        object GetObjectByKey(EntityKey key);
        void AddChangeSet(TChangeSet changeSet);
    }
}
