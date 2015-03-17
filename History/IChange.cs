using System;
using System.Collections.Generic;
using FrameLog.Patterns.Models;

namespace FrameLog.History
{
    public interface IChange<TValue, TPrincipal>
    {
        DateTime Timestamp { get; }
        TPrincipal Author { get; }
        TValue Value { get; }

        IObjectChange<TPrincipal> ObjectChange { get; }
        
        IEnumerable<Exception> Errors { get; }
        bool ProblemsRetrievingData { get; }
    }
}
