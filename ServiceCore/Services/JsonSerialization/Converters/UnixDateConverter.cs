using System;
using Newtonsoft.Json;

namespace ServiceCore.Services.JsonSerialization.Converters
{
    public class UnixDateConverter : JsonConverter<DateTime?>
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
        {
            long? unixTime = null;
            if (value != null)
            {
                var dateToConvert = value.Value;
                var dateTimeOffset = new DateTimeOffset(dateToConvert);
                unixTime = dateTimeOffset.ToUnixTimeMilliseconds();
            }
            serializer.Serialize(writer, unixTime);
        }

        /// <inheritdoc />
        public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var unixTime = serializer.Deserialize<long?>(reader);
            if (unixTime == null)
                return null;

            var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTime.Value);
            var date = dateTimeOffset.DateTime.ToUniversalTime();
            return date;
        }
    }
}