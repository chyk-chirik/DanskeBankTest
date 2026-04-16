using DanskeBankTest.Services.Types;
using Microsoft.Extensions.Caching.Memory;
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
               async ct => await GetLatestRates(ct),
               new FusionCacheEntryOptions
               {
                   Duration = TimeSpan.FromSeconds(options.Value.CacheInSeconds!.Value),
                   IsFailSafeEnabled = true,
                   FailSafeMaxDuration = TimeSpan.FromHours(2), // issue must be fixed within 2 hours, otherwise financial risk is high 
                   FailSafeThrottleDuration = TimeSpan.FromSeconds(options.Value.CacheInSeconds!.Value / 3), // here I assume cache time is more then 3 secs always
                   EagerRefreshThreshold = 0.9f,
                   FactorySoftTimeout = TimeSpan.Zero
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
           
            var rates = FreeCurrencyRateDeserializer.Deserialize(responseStream);
            return new RateData
            {
                Data = rates,
                BaseCurrency = options.Value.BaseCurrency
            };
        }
    }
}
