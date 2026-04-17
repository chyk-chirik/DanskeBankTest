using DanskeBankTest.Services.ExchangeRate.FreeCurrencyApi;
using DanskeBankTest.Services.Types;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace DanskeBankTest.Services.Tests
{
    [TestClass]
    public sealed class FreeCurrencyApiDeserializationTests
    {
        [TestMethod]
        public void Deserialize_ResponsePayloadDeserialized()
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
            var data = FreeCurrencyRateDeserializer.Deserialize(stream);
           
            data.ShouldNotBeNull();
            data.ShouldNotBeEmpty();
            data[Currency.DKK].ShouldBe(1);
            data[Currency.EUR].ShouldBe(1.5m);
        }
    }
}
