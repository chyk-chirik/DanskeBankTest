using DanskeBankTest.Services.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services.ExchangeRate
{
    public class ExchangeRateOptions
    {
        public Currency BaseCurrency { get; set; }
        public int CacheInSeconds { get; set; }
    }
}
