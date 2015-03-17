using System;
using System.Collections.Generic;
using FrameLog.Contexts;
using FrameLog.Translation.Binders;
using FrameLog.Translation.ValueTranslators;

namespace FrameLog.Translation
{
    [Obsolete("\"LegacyBindManager\" is deprecated and will be removed in future versions of FrameLog. " +
        "Consider switching to \"ValueTranslationManager\", which supports both value serialization and value binding.")]
    public class LegacyBindManager : IBindManager
    {
        protected List<IBinder> binders;
        private IHistoryContext db;

        public LegacyBindManager(IHistoryContext db)
        {
            this.db = db;
            binders = new List<IBinder>()
            {
                new PrimitiveTranslator(),
                new GuidTranslator(),
                new DateTimeTranslator(legacyMode: true),
                new NullableBinder(this),
                new CollectionTranslator(this, null, db),
            };
        }

        public virtual TValue Bind<TValue>(string reference, object existingValue = null)
        {
            return (TValue)Bind(reference, typeof(TValue), existingValue);
        }
        public virtual object Bind(string raw, Type type, object existingValue = null)
        {
            foreach (var binder in binders)
            {
                if (binder.Supports(type))
                    return binder.Bind(raw, type, existingValue);
            }
            if (raw == null)
                return null;
            
            return db.GetObjectByReference(type, raw);
        }
    }
}
