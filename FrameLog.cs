using System;
using System.Data.Entity.Core.Objects;
using FrameLog.Patterns.Filter;
using FrameLog.Patterns.Models;
using FrameLog.Logging;
using FrameLog.Patterns.Logging;
using FrameLog.Filter;
using FrameLog.Translation.Serializers;

namespace FrameLog
{
    public class FrameLog<TChangeSet, TPrincipal>
        where TChangeSet : IChangeSet<TPrincipal>
    {
        public bool Enabled { get; set; }
        private ContextInfo contextInfo;
        private IChangeSetFactory<TChangeSet, TPrincipal> factory;
        private ILoggingFilter filter;
        private ISerializationManager serializer;

        private ChangeLogger<TChangeSet, TPrincipal> logger;
        private IOven<TChangeSet, TPrincipal> oven;

        public FrameLog(ContextInfo contextInfo,
            IChangeSetFactory<TChangeSet, TPrincipal> factory,
            ILoggingFilterProvider filter = null,
            ISerializationManager serializer = null)
        {
            this.contextInfo = contextInfo;
            this.factory = factory;
            this.filter = (filter ?? Filters.Default).Get(contextInfo);
            // this.serializer = (serializer ?? new ValueTranslationManager(contextInfo.Context));
            Enabled = true;
        }

        public void logChanges(TPrincipal principal)
        {
            logger = new ChangeLogger<TChangeSet, TPrincipal>(contextInfo.Context, factory, filter, serializer);

            // This returns the log objects, but they are not attached to the context
            // so the context change tracker hasn't noticed them
            oven = logger.Log(contextInfo.ObjectContext.ObjectStateManager);

            // So when we accept changes, we are only accepting the changes from the
            // original changes - the context hasn't yet detected the log changes
            // contextInfo.ObjectContext.AcceptAllChanges();


            // return default(TChangeSet);
        }

        public TChangeSet getLogs(TPrincipal principal)
        {
            // This code then attaches the log objects to the context
            if (oven.HasChangeSet)
            {
                // First do any deferred log value calculations.
                // See PropertyChange.Bake for more information
                TChangeSet changeSet = oven.Bake(DateTime.Now, principal);
                return changeSet;
                // LogContext.ChangeSets.Add(changeSet);
                // context.AddChangeSet(changeSet);
            }
            return default(TChangeSet);
        }

        private static readonly SaveOptions detectAndAccept =
            SaveOptions.DetectChangesBeforeSave | SaveOptions.AcceptAllChangesAfterSave;
    }
}
