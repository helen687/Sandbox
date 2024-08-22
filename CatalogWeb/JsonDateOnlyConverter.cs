using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace CatalogWeb
{
    public class JsonDateOnlyConverter : JsonConverter<DateOnly>
    {
        // Define the date format the data is in
        private const string DateFormat = "yyyy-mm-dd";

        // This is the deserializer
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Exception exception = null;
            try
            {
                return DateOnly.ParseExact(reader.GetString()!,
                    DateFormat);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            try
            {
                var dt = DateTime.ParseExact(reader.GetString()!,
                    "yyyy-MM-ddThh:mm:ss.fffZ", CultureInfo.InvariantCulture);
                return DateOnly.FromDateTime(dt);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            throw exception;
        }

        // This is the serializer
        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
