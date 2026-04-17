using DanskeBankTest.Services.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services.ExchangeRate.FreeCurrencyApi
{
    public class FreeCurrencyApiOptions
    {
        public string BaseUrl { get; set; } = null!;
        public string ApiKey { get; set; } = null!;
        // intermidiate currencey to use when direct exchange rate is not available
        public Currency BaseCurrency { get; set; }
        public int CacheInSeconds { get; set; }
    }
}
