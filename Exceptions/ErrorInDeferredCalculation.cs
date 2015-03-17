using System;

namespace FrameLog.Exceptions
{
    public class ErrorInDeferredCalculation : Exception
    {
        public ErrorInDeferredCalculation(Exception innerException)
            : base(message(innerException), innerException) { }

        public ErrorInDeferredCalculation(object container, string key, Exception innerException)
            : base(messageFor(container, key, innerException), innerException) { }

        private static string message(Exception innerException)
        {
            return string.Format("An error of type '{2}' occured during deferred calculation of a value. See inner exception for more details.",
                innerException.GetType());
        }
        private static string messageFor(object container, string key, Exception innerException)
        {
            return string.Format("An error of type '{2}' occured during deferred calculation of property '{0}' on container '{1}'. See inner exception for more details.",
                key, container, innerException.GetType());
        }
    }
}
