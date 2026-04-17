using DanskeBankTest.Services.Types;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using ZiggyCreatures.Caching.Fusion;

namespace DanskeBankTest.Services.ExchangeRate.FreeCurrencyApi
{
    public class FreeCurrencyApiProvider(
        HttpClient http, 
        IOptions<FreeCurrencyApiOptions> options,
        ILogger<FreeCurrencyApiProvider> logger,
        IFusionCache cache) : IExchangeRateService
    {
        private static string AllCurrencies = Uri.EscapeDataString(string.Join(",", Enum.GetValues<Currency>()));

        public async Task<CurrencyRate> GetExchangeRate(CurrencyPair currencyPair, CancellationToken ct)
        {
            if (currencyPair.IsSameCurrency)
            {
                return new CurrencyRate(currencyPair, 1m);
            }

            var rateData = await cache.GetOrSetAsync(
               $"FreeCurrencyApiProvider_{options.Value.BaseCurrency}",
               GetLatestRates,
               new FusionCacheEntryOptions
               {
                   Duration = TimeSpan.FromSeconds(options.Value.CacheInSeconds), // well, in real life all values will be moved to config, given config usage just an example
                   IsFailSafeEnabled = true,
                   FailSafeMaxDuration = TimeSpan.FromHours(2), // issue must be fixed within 2 hours, otherwise financial risk is high 
                   FailSafeThrottleDuration = TimeSpan.FromSeconds(2),
                   EagerRefreshThreshold = 0.9f,
                   FactorySoftTimeout = TimeSpan.FromMicroseconds(100),
                   FactoryHardTimeout = TimeSpan.FromSeconds(4)
               },
               ct
           );

            var moneyCurrencyRate = new CurrencyRate(new CurrencyPair(rateData.BaseCurrency, currencyPair.MoneyCurrency), rateData.Data[currencyPair.MoneyCurrency]);

            if (rateData.BaseCurrency == currencyPair.MainCurrency)
            {
                return moneyCurrencyRate;
            }
            else
            {
                var mainCurrencyRate = new CurrencyRate(new CurrencyPair(rateData.BaseCurrency, currencyPair.MainCurrency), rateData.Data[currencyPair.MainCurrency]);
                return CurrencyRate.GetRelativeMoneyRate(mainCurrencyRate, moneyCurrencyRate);
            }
        }


        private async Task<RateData> GetLatestRates(CancellationToken ct)
        {
            using var response = await http.GetAsync($"latest?base_currency={options.Value.BaseCurrency}&currencies={AllCurrencies}", ct);
            using var responseStream = await response.Content.ReadAsStreamAsync(ct);

            logger.LogDebug("Received respnse status {Status} from Free Currency Api provider for currency {BaseCurrency}", response.StatusCode, options.Value.BaseCurrency);

            var rates = FreeCurrencyRateDeserializer.Deserialize(responseStream);

            return new RateData
            {
                Data = rates,
                BaseCurrency = options.Value.BaseCurrency
            };
        }
    }
}
