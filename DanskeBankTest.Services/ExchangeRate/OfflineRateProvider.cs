using DanskeBankTest.Services.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services.ExchangeRate
{
    public class OfflineRateProvider : IExchangeRateService
    {
        public ValueTask<CurrencyRate> GetExchangeRate(CurrencyPair currencyPair, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
