using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoodAdvisor.Models
{
    class JsonDoubleToDateConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(reader.GetDouble());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            writer.WriteNumberValue((((DateTime)value).ToUniversalTime() - epoch).TotalMilliseconds);
        }
    }
}
