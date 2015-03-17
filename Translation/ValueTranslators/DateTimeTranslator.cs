using System;
using System.Globalization;
using FrameLog.Translation.Binders;
using FrameLog.Translation.Serializers;

namespace FrameLog.Translation.ValueTranslators
{
    public class DateTimeTranslator : IBinder, ISerializer
    {
        private readonly bool legacyMode;

        internal DateTimeTranslator(bool legacyMode)
        {
            this.legacyMode = legacyMode;
        }

        public DateTimeTranslator() : this(false) { }

        public bool Supports(Type type)
        {
            return typeof(DateTime?).IsAssignableFrom(type);
        }

        public object Bind(string raw, Type type, object existingValue)
        {
            try
            {
                if (raw == null)
                    return default(DateTime);

                // Check if the date time is represents as numeric ticks
                if (!legacyMode)
                {
                    long ticks;
                    if (long.TryParse(raw, out ticks))
                        return new DateTime(ticks);
                }

                // Else, try parse the date as a string
                // NOTE: This is required to support legacy dates stored using FrameLog <= 1.5.0 where the dates 
                //       are bound as localised strings. Localised strings can be interrepted differently depending 
                //       on the current culture of the machine. It is more reliable to store dates as numeric ticks 
                //       because the format of an integer is not subjected to localisation issues.
                return (legacyMode)
                    ? DateTime.Parse(raw)
                    : DateTime.Parse(raw, CultureInfo.InvariantCulture);
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
            var dateTime = (DateTime)obj;
            return (legacyMode)
                ? dateTime.ToString()
                : dateTime.Ticks.ToString(CultureInfo.InvariantCulture);
        }
    }
}
