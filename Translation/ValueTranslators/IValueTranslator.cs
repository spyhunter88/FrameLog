using System;

namespace FrameLog.Translation.ValueTranslators
{
    public interface IValueTranslator
    {
        bool Supports(Type type);
    }
}
