using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CopyRestAPI.Helpers
{
    public class UnixDateTimeConverter : DateTimeConverterBase
    {
        private static readonly DateTime Epoc = new DateTime(1970, 1, 1);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value.ToString();

            long secondsToAdd;

            if (long.TryParse(value, out secondsToAdd))
            {
                return Epoc.AddSeconds(secondsToAdd);
            }

            DateTime date;
            if (DateTime.TryParse(value, out date))
            {
                return date;
            }

            throw new Exception("Expected value of type long or DateTime. Got value: " + value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long ticks;
            if (value is DateTime)
            {
                var delta = ((DateTime)value) - Epoc;
                if (delta.TotalSeconds < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Unix epoc starts January 1st, 1970");
                }
                ticks = (long)delta.TotalSeconds;
            }
            else
            {
                throw new Exception("Expected date object value.");
            }
            writer.WriteValue(ticks);
        }
    }
}
