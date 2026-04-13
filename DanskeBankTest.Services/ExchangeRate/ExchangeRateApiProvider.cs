using DanskeBankTest.Services.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services.ExchangeRate
{
    public class ExchangeRateApiProvider : IExchangeRateService
    {
        public ValueTask<CurrencyRate> GetExchangeRate(Currency mainCurrency, Currency moneyCurrency, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
