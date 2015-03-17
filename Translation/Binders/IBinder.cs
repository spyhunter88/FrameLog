using System;
using FrameLog.Translation.ValueTranslators;

namespace FrameLog.Translation.Binders
{
    public interface IBinder : IValueTranslator
    {
        object Bind(string raw, Type type, object existingValue = null);
    }
}
