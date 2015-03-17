using FrameLog.Translation.ValueTranslators;

namespace FrameLog.Translation.Serializers
{
    public interface ISerializer : IValueTranslator
    {
        string Serialize(object obj);
    }
}
