using DanskeBankTest.FreeCurrencyApiClient;
using DanskeBankTest.Services.Types;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace DanskeBankTests.FreeCurrencyApiClient.Tests
{
    [TestClass]
    public sealed class RateConverterTests
    {
        [TestMethod]
        public void RateConverter_FreeApiRateResponsePayload_DictionaryCorrectlyDeserialized()
        {
            var payload = """
                {
                  "data": {
                    "DKK": 1,
                    "EUR": 1.5
                  }
                }
                """;

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(payload));
            var options = new JsonSerializerOptions
            {
                Converters = { new RateConverter() }
            };

            // Passing the options (with the custom converter) here
            var data = JsonSerializer.Deserialize<Dictionary<string, decimal>>(stream, options);

            data.ShouldNotBeNull();
            data.ShouldNotBeEmpty();
            data["DKK"].ShouldBe(1);
            data["EUR"].ShouldBe(1.5m);
        }
    }
}
