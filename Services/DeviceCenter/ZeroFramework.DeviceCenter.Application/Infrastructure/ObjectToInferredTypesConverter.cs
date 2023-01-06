using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZeroFramework.DeviceCenter.Application.Infrastructure
{
    public class ObjectToInferredTypesConverter : JsonConverter<object?>
    {
        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Number when reader.TryGetInt32(out int value) => value,
            JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
            JsonTokenType.Number => reader.GetDouble(),
            JsonTokenType.String when reader.TryGetDateTimeOffset(out DateTimeOffset dateTimeOffset) => dateTimeOffset,
            JsonTokenType.String when reader.TryGetDateTime(out DateTime datetime) => datetime,
            JsonTokenType.String => reader.GetString(),
            _ => JsonDocument.ParseValue(ref reader).RootElement.Clone()
        };

        public override void Write(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
        {
            if (value is not null)
            {
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
                return;
            }

            writer.WriteNullValue();
        }
    }
}
