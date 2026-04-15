using DanskeBankTest.Services.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DanskeBankTest.Services.ExchangeRate.FreeCurrencyApi
{
    internal class RateData
    {
        [JsonIgnore]
        public Currency BaseCurrency { get; set; }

        public Dictionary<Currency, decimal> Data { get; set; } = null!;
    }
}
