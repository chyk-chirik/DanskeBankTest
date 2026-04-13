using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services.ExchangeRate
{
    public class StaticRateProvider : IExchangeRateService
    {
        public ValueTask<CurrencyRate> GetExchangeRate(Currency mainCurrency, Currency moneyCurrency, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
