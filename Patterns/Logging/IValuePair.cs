using System.Data.Entity;

namespace FrameLog.Patterns.Logging.ValuePairs
{
    public interface IValuePair
    {
        bool HasChanged { get; }
        string PropertyName { get; }
        object OriginalValue { get; }
        object NewValue { get; }
        EntityState State { get; }
    }
}
