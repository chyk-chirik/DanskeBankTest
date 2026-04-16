using DanskeBankTest.Services.Types;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;

namespace DanskeBankTest.Services.ExchangeRate.FreeCurrencyApi
{
    public class FreeCurrencyApiProvider(
        HttpClient http, 
        IOptions<FreeCurrencyApiOptions> options,
        IMemoryCache cache) : IExchangeRateService
    {
        private static string AllCurrencies = Uri.EscapeDataString(string.Join(",", Enum.GetValues<Currency>()));

        private string CacheKey = $"FreeCurrencyApiProvider_{options.Value.BaseCurrency}";

        public async Task<CurrencyRate> GetExchangeRate(CurrencyPair currencyPair, CancellationToken ct)
        {
            var rateData = options.Value.CacheInSeconds.HasValue
                ? await GetFromCache(currencyPair, ct)
                : await GetLatestRates(ct);

            var moneyCurrencyRate = new CurrencyRate(currencyPair, rateData.Data[currencyPair.MoneyCurrency]);

            if (rateData.BaseCurrency == currencyPair.MainCurrency)
            {
                return moneyCurrencyRate;
            }
            else
            {
                var mainCurrencyRate = new CurrencyRate(currencyPair, rateData.Data[currencyPair.MainCurrency]);

                return CurrencyRate.FromRelativeRate(mainCurrencyRate, moneyCurrencyRate);
            }
        }

        private Task<RateData> GetFromCache(CurrencyPair currencyPair, CancellationToken ct)
        {
            return cache.GetOrCreateAsync(CacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(options.Value.CacheInSeconds!.Value);

                return await GetLatestRates(ct);
            })!;
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
