using System;

namespace FrameLog.Exceptions
{        
    public class FailedToRetrieveObjectException : Exception
    {
        public readonly Type Type;
        public readonly string Reference;

        public const string DefaultMessage = "Failed to retrieve object identified by type '{0}' and reference '{1}'";

        /// <param name="type">The type of the object.</param>
        /// <param name="reference">The FrameLog reference that identifies it. See https://bitbucket.org/MartinEden/framelog/wiki/ObjectReferences </param>
        /// <param name="message">
        /// By default this is DefaultMessage. You can substitute any string. 
        /// The following substitutions will be made:
        /// {0}: The type
        /// {1}: The reference
        /// </param>
        public FailedToRetrieveObjectException(Type type, string reference, 
            Exception innerException = null, string message = null)
            : base(string.Format(message ?? DefaultMessage, type, reference), innerException)
        {
            Type = type;
            Reference = reference;
        }
    }
}
