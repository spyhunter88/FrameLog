using System;
using FrameLog.Translation.Binders;
using FrameLog.Translation.Serializers;

namespace FrameLog.Translation.ValueTranslators
{
    public class PrimitiveTranslator : IBinder, ISerializer
    {
        public bool Supports(Type type)
        {
            return type.IsPrimitive 
                || type == typeof(string)
                || type == typeof(decimal);
        }

        public object Bind(string raw, Type type, object existingValue)
        {
            if (raw == null)
                return null;

            return Convert.ChangeType(raw, type);
        }

        public string Serialize(object obj)
        {
            return (obj != null ? obj.ToString() : null);
        }
    }
}
