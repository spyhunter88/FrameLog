using FrameLog.Exceptions;
using System;
using System.Linq;
using System.Reflection;

namespace FrameLog.History
{
    public static class HistoryHelpers
    {
        public static object Instantiate(Type type)
        {
            if (type == null)
                return null;

            try
            {
                var parameterlessConstructor = type
                    .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(c => !c.GetParameters().Any())
                    .FirstOrDefault();
                if (parameterlessConstructor != null)
                    return parameterlessConstructor.Invoke(null);

                throw new UnableToInstantiateObjectException(type);
            }
            catch (Exception ex)
            {
                throw new UnableToInstantiateObjectException(type, ex);
            }
        }
        public static T Instantiate<T>()
        {
            return (T)Instantiate(typeof(T));
        }
    }
}
