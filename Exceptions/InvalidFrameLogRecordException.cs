using System;

namespace FrameLog.Exceptions
{
    public class InvalidFrameLogRecordException : Exception
    {
        public readonly object Record;

        public InvalidFrameLogRecordException(string message, object record)
            : base(string.Format(message, record))
        {
            Record = record;
        }
    }
}
