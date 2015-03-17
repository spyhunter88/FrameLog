using System;
using FrameLog.Models;
using FrameLog.Patterns.Models;

namespace FrameLog.History
{
    public class UnknownPropertyInLogException<TPrincipal> : Exception
    {
        public readonly IPropertyChange<TPrincipal> PropertyChange;

        public UnknownPropertyInLogException(IPropertyChange<TPrincipal> propertyChange)
        {
            PropertyChange = propertyChange;
        }

        public override string Message
        {
            get
            {
                return string.Format("When retrieving history, a IPropertyChange naming property '{0}' was found in the log that did not correspond to any property on the model, which had type '{1}'. This may be because a property has been removed from the model, or renamed, since the log record was created. Consider migrating your log records when you make schema changes to allow strongly-typed retrieval historic records.",
                    PropertyChange.PropertyName, PropertyChange.ObjectChange.TypeName);
            }
        }
    }
}
