using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DanskeBankTest.FreeCurrencyApiClient
{
    public class RateConverter : JsonConverter<Dictionary<string, decimal>>
    {
        public override Dictionary<string, decimal>? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);

            if (doc.RootElement.TryGetProperty("data", out var dataElement))
            {
                return dataElement.EnumerateObject().ToDictionary(k => k.Name, v => v.Value.GetDecimal());
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<string, decimal> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
