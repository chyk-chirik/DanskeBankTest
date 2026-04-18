using Castle.Core.Logging;
using DanskeBankTest.Services.ExchangeRate;
using DanskeBankTest.FreeCurrencyApiClient;
using DanskeBankTest.Services.Types;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml.Linq;
using ZiggyCreatures.Caching.Fusion;

namespace DanskeBankTest.Services.Tests
{
    [TestClass]

    public sealed class FreeCurrencyApiTests
    {
        [TestMethod]
        [DataRow(Currency.DKK, Currency.EUR, Currency.USD)]
        [DataRow(Currency.DKK, Currency.DKK, Currency.USD)]
        [DataRow(Currency.DKK, Currency.EUR, Currency.DKK)]
        public async Task GetExchangeRate_ReceivedCorrectConversion(Currency baseCurrency, Currency mainCurrency, Currency moneyCurrency)
        {

            var mainRate = baseCurrency == mainCurrency ? 1 : 2.1m;
            var moneyRate = baseCurrency == moneyCurrency ? 1 : 3.4m;

            var apiClient = GetFreeCurrencyApiClient(new Dictionary<string, decimal> { 
                { mainCurrency.ToString(), mainRate }, 
                { moneyCurrency.ToString(), moneyRate } 
            });
            var memoryCache = new FusionCache(new FusionCacheOptions());
            var logger = NullLogger<ExchangeRateService>.Instance;
            var settings = Options.Create(new ExchangeRateOptions
            {
                BaseCurrency = baseCurrency,
                CacheInSeconds = 60
            });
            var rateService = new ExchangeRateService(apiClient, settings, logger, memoryCache);

            var currencyPair = new CurrencyPair(mainCurrency, moneyCurrency);
            var rate = rateService.GetExchangeRate(currencyPair, CancellationToken.None)
                .ShouldNotThrow();

            rate.CurrencyPair.ShouldBe(currencyPair);
            rate.Rate.ShouldBe(moneyRate / mainRate);
        }

        private static IFreeCurrencyApiClient GetFreeCurrencyApiClient(Dictionary<string, decimal> fakeData)
        {
            var fakeResp = new FreeCurrenceApiResponse<Dictionary<string, decimal>>(fakeData, true, 200);

            var mockClient = new Mock<IFreeCurrencyApiClient>();

            mockClient
                .Setup(x => x.GetRates(
                    It.IsAny<string>(),
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeResp);

            return mockClient.Object;
        }
    }
}
