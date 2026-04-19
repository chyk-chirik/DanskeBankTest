using DanskeBankTest.FreeCurrencyApiClient;
using DanskeBankTest.Services.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZiggyCreatures.Caching.Fusion;

namespace DanskeBankTest.Services.ExchangeRate
{
    public interface IExchangeRateService
    {
        Task<CurrencyRate> GetExchangeRate(CurrencyPair currencyPair, CancellationToken ct);
    }

    public class ExchangeRateService(
       IFreeCurrencyApiClient freeCurrencyApiClient,
       IOptions<ExchangeRateOptions> options,
       ILogger<ExchangeRateService> logger,
       IFusionCache cache) : IExchangeRateService
    {
        private static string[] AllCurrencies = Enum.GetValues<Currency>().Select(c => c.ToString()).ToArray();

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
                   Duration = TimeSpan.FromSeconds(options.Value.CacheDurationInSeconds),
                   IsFailSafeEnabled = true,
                   FailSafeMaxDuration = TimeSpan.FromMinutes(options.Value.FailSafeMaxDurationInMinutes), // issue must be fixed within given period, otherwise financial risk is high 
                   FailSafeThrottleDuration = TimeSpan.FromSeconds(options.Value.FailSafeThrottleDurationInSeconds),
                   EagerRefreshThreshold = 0.9f,
                   FactorySoftTimeout = TimeSpan.FromMilliseconds(options.Value.FactorySoftTimeoutInMilliseconds),
                   FactoryHardTimeout = TimeSpan.FromSeconds(options.Value.FactoryHardTimeoutInSeconds)
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
            var baseCurrency = options.Value.BaseCurrency;
            // allocation can be avoided if necessary, we can keep a map string->currency and vice versa
            var data = await freeCurrencyApiClient.GetRates(baseCurrency.ToString(), AllCurrencies, ct);

            if(!data.IsSuccess)
            {
                throw new Exception($"Failed to get exchange rates from FreeCurrencyApi. StatusCode: {data.StatusCode}, ErrorMessage: {data.ErrorMessage}");
            }

            return new RateData
            {
                Data = data.Response!.ToDictionary(kvp => Enum.Parse<Currency>(kvp.Key), kvp => kvp.Value),
                BaseCurrency = options.Value.BaseCurrency
            };
        }
    }
}
