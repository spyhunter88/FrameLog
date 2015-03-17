﻿namespace FrameLog.Patterns.Filter
{
    /// <summary>
    /// Constructs a logging filter from a given IFrameLogContext.
    /// For each ILoggingFilter, there is a corresponding provider.
    /// </summary>
    public interface ILoggingFilterProvider
    {
        ILoggingFilter Get(ContextInfo context);
    }
}
