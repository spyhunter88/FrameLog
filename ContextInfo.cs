using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

namespace FrameLog
{
    public class ContextInfo
    {
        public Type UnderlyingContextType { get; set; }
        public MetadataWorkspace Workspace { get; set; }

        public DbContext Context { get; set; }
        public ObjectContext ObjectContext { get { return ((System.Data.Entity.Infrastructure.IObjectContextAdapter)Context).ObjectContext; } }
    }
}
