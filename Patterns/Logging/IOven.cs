using System;
using FrameLog.Patterns.Models;

namespace FrameLog.Patterns.Logging
{
    internal interface IOven<TChangeSet, TPrincipal>
        where TChangeSet : IChangeSet<TPrincipal>
    {
        bool HasChangeSet { get; }
        TChangeSet Bake(DateTime timestamp, TPrincipal author);
    }
}
