using System.Text.Json;
using System.Text.Json.Serialization;
using PennForgeTech.Core.Abstractions;
using PennForgeTech.Core.Primitives;

namespace PennForgeTech.Core.Serialization.Json
{
    public sealed class JsonTextSerializer : ISerializer
    {
        private static readonly JsonSerializerOptions Options = CreateOptions();

        public string Serialize<TValue>(TValue value)
        {
            try
            {
                return JsonSerializer.Serialize(value, Options);
            }
            catch (JsonException exception)
            {
                throw new SerializationFailedException("The value could not be converted to JSON.", exception);
            }
        }

        public TValue Deserialize<TValue>(string text)
        {
            Guard.AgainstNullOrWhiteSpace(text, nameof(text));

            TValue? value;

            try
            {
                value = JsonSerializer.Deserialize<TValue>(text, Options);
            }
            catch (JsonException exception)
            {
                throw new SerializationFailedException("The text is not valid JSON for the requested type.", exception);
            }

            if (value is null)
            {
                throw new SerializationFailedException("The JSON text produced no value.");
            }

            return value;
        }

        private static JsonSerializerOptions CreateOptions()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }
    }
}
