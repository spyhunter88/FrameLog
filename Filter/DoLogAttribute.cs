using System;
using FrameLog.Patterns.Filter;

namespace FrameLog.Filter
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class DoLogAttribute : Attribute, IFilterAttribute
    {
        public bool ShouldLog()
        {
            return true;
        }
    }
}
