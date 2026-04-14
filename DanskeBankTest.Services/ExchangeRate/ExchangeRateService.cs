using DanskeBankTest.Services.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services.ExchangeRate
{
    public class ExchangeRateService(
        [FromKeyedServices(KeyedServicesNames.ExchangeRateRealtime)] IEnumerable<IExchangeRateService> realTimeRateServices,
        [FromKeyedServices(KeyedServicesNames.ExchangeRateOffline)] IExchangeRateService offlineRateService) : IExchangeRateService
    {
        public ValueTask<CurrencyRate> GetExchangeRate(CurrencyPair currencyPair, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
