using System;
using System.Linq;
using FrameLog.Translation.Binders;

namespace FrameLog.Translation.ValueTranslators
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is intentionally not a serializer, only a binder. It makes no sense to serialize a nullable from an object => string.
    /// If there is a serializer registered that can handle the object, it would also be able to handle nulls/defaults of that object type.
    /// </remarks>
    public class NullableBinder : IBinder
    {
        private IBindManager bindManager;

        public NullableBinder(IBindManager bindManager)
        {
            this.bindManager = bindManager;
        }

        public bool Supports(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public object Bind(string raw, Type type, object existingValue)
        {
            if (raw == null)
                return null;

            return bindManager.Bind(raw, underlyingType(type), existingValue);
        }

        private Type underlyingType(Type type)
        {
            return type.GetGenericArguments().Single();
        }
    }
}
