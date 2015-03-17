using System;

namespace FrameLog.History
{
    public class CreationDoesNotExistInLogException : Exception
    {
        public readonly object Model;

        public CreationDoesNotExistInLogException(object model)
            : base(string.Format("There is no record of this object's creation in the log. Object: '{0}'.", model))
        {
            Model = model;
        }
    }
}
