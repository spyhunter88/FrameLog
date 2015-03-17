using System;

namespace FrameLog.Logging
{
    public interface IChangeType
    {
        bool IsA(Type type);
    }

    public static class ChangeTypeExtensions
    {
        public static IChangeType GetChangeType(this object obj)
        {
            if (obj == null)
                return new UnknownChangeType();
            else
                return new ConcreteChangeType(obj.GetType());
        }
    }

    public class ConcreteChangeType : IChangeType
    {
        private Type wrappedType;

        public ConcreteChangeType(Type type)
        {
            this.wrappedType = type;
        }

        public bool IsA(Type type)
        {
            return type.IsAssignableFrom(wrappedType);
        }
    }

    public class UnknownChangeType : IChangeType
    {
        public bool IsA(Type type)
        {
            return false;
        }
    }
}
