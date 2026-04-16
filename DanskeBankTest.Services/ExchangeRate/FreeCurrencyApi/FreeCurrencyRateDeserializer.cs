using DanskeBankTest.Services.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DanskeBankTest.Services.ExchangeRate.FreeCurrencyApi
{
    public class FreeCurrencyRateDeserializer
    {
        public static Dictionary<Currency, decimal> Deserialize(Stream stream)
        {
            var result = new Dictionary<Currency, decimal>();

            using (JsonDocument doc = JsonDocument.Parse(stream))
            {
                var data = doc.RootElement.GetProperty("data");

                foreach (JsonProperty property in data.EnumerateObject())
                {
                    var currencyCode = Enum.Parse<Currency>(property.Name);

                    result[currencyCode] = property.Value.GetDecimal();
                }
            }

            return result;
        }
    }
}
