using System;
using System.Globalization;
using FrameLog.Translation.Binders;
using FrameLog.Translation.Serializers;

namespace FrameLog.Translation.ValueTranslators
{
    public class TimeSpanTranslator : IBinder, ISerializer
    {
        public bool Supports(Type type)
        {
            return typeof(TimeSpan?).IsAssignableFrom(type);
        }

        public object Bind(string raw, Type type, object existingValue)
        {
            try
            {
                if (raw == null)
                    return default(TimeSpan);

                // Check if the time span is represents as numeric ticks
                long ticks;
                if (long.TryParse(raw, out ticks))
                    return new TimeSpan(ticks);

                // Else, try parse the time span as a string
                // NOTE: This is a fallback incase the time span is stored as string. It is more reliable to store 
                //       spans as numeric ticks because the format of an integer is not subjected to localisation issues.
                return TimeSpan.Parse(raw, CultureInfo.InvariantCulture);
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public string Serialize(object obj)
        {
            var timeSpan = (TimeSpan)obj;
            return timeSpan.Ticks.ToString(CultureInfo.InvariantCulture);
        }
    }
}
