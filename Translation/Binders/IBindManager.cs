using System;

namespace FrameLog.Translation.Binders
{
    public interface IBindManager
    {
        ItemType Bind<ItemType>(string raw, object existingValue = null);
        object Bind(string raw, Type type, object existingValue = null);
    }
}
