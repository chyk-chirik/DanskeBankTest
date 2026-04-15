using DanskeBankTest.Services.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services.ExchangeRate
{
    public class ExchangeRateFacadeService(
        [FromKeyedServices(KeyedServicesNames.ExchangeRateRealtime)] IExchangeRateService realtimeRateService,
        [FromKeyedServices(KeyedServicesNames.ExchangeRateOffline)] IExchangeRateService offlineRateService)
        : IExchangeRateService
    {
        public async ValueTask<CurrencyRate> GetExchangeRate(CurrencyPair currencyPair, CancellationToken ct)
        {
            try
            {
                return await realtimeRateService.GetExchangeRate(currencyPair, ct);
            }
            catch (Exception ex)
            {
                return await offlineRateService.GetExchangeRate(currencyPair, ct);
            }
        }
    }
}
