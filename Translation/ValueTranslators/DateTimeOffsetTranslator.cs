using System;
using System.Globalization;
using System.Linq;
using FrameLog.Translation.Binders;
using FrameLog.Translation.Serializers;

namespace FrameLog.Translation.ValueTranslators
{
    public class DateTimeOffsetTranslator : IBinder, ISerializer
    {
        public bool Supports(Type type)
        {
            return typeof(DateTimeOffset?).IsAssignableFrom(type);
        }

        public object Bind(string raw, Type type, object existingValue)
        {
            try
            {
                if (raw == null)
                    return default(DateTimeOffset);

                // Check if the date time is represents as numeric ticks
                var offsetParts = raw.Split('+');
                if (offsetParts.Length == 2)
                {
                    long dateTicks, offsetTicks;
                    if (long.TryParse(offsetParts.First(), out dateTicks) && 
                        long.TryParse(offsetParts.Last(), out offsetTicks))
                        return new DateTimeOffset(new DateTime(dateTicks), new TimeSpan(offsetTicks));
                }

                // Else, try parse the date as a string
                // NOTE: This is a fallback incase the date time offset is stored as string. It is more reliable to store 
                //       offsets as numeric ticks because the format of an integer is not subjected to localisation issues.
                return DateTimeOffset.Parse(raw, CultureInfo.InvariantCulture);
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
            var dateTimeOffset = (DateTimeOffset)obj;
            var dateTicks = dateTimeOffset.Ticks.ToString(CultureInfo.InvariantCulture);
            var offsetTicks = dateTimeOffset.Offset.Ticks.ToString(CultureInfo.InvariantCulture);
            return String.Format("{0}+{1}", dateTicks, offsetTicks);
        }
    }
}
