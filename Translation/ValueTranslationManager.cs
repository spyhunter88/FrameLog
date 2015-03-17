using System;
using System.Collections.Generic;
using System.Linq;
using FrameLog.Contexts;
using FrameLog.Translation.Binders;
using FrameLog.Translation.Serializers;
using FrameLog.Translation.ValueTranslators;

namespace FrameLog.Translation
{
    public partial class ValueTranslationManager : ISerializationManager, IBindManager
    {
        protected List<IValueTranslator> translators;
        private IHistoryContext db;

        public ValueTranslationManager(IHistoryContext db)
        {
            this.db = db;
            translators = new List<IValueTranslator>()
            {
                new PrimitiveTranslator(),
                new GuidTranslator(),
                new DateTimeTranslator(),
                new DateTimeOffsetTranslator(),
                new TimeSpanTranslator(),
                new EnumTranslator(),
                new BinaryBlobTranslator(),
                new NullableBinder(this),
                new CollectionTranslator(this, this, db),
            };
        }

        public virtual TValue Bind<TValue>(string raw, object existingValue = null)
        {
            return (TValue)Bind(raw, typeof(TValue), existingValue);
        }
        public virtual object Bind(string raw, Type type, object existingValue = null)
        {
            if (raw == null)
                return null;
            
            foreach (var binder in translators.OfType<IBinder>())
            {
                if (binder.Supports(type))
                    return binder.Bind(raw, type, existingValue);
            }
            return db.GetObjectByReference(type, raw);
        }
        public string Serialize(object obj)
        {
            if (obj == null)
                return null;

            var type = obj.GetType();
            foreach (var serializer in translators.OfType<ISerializer>())
            {
                if (serializer.Supports(type))
                    return serializer.Serialize(obj);
            }
            return obj.ToString();
        }
    }
}
