using DanskeBankTest.Services.ExchangeRate;
using DanskeBankTest.Services.ExchangeRate.FreeCurrencyApi;
using DanskeBankTest.Services.Types;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace DanskeBankTest.Services.Tests
{
    [TestClass]

    public sealed class FreeCurrencyApiWithCacheTests
    {
        [TestMethod]
        [DataRow(Currency.DKK, Currency.EUR, Currency.USD)]
        [DataRow(Currency.DKK, Currency.DKK, Currency.USD)]
        [DataRow(Currency.DKK, Currency.EUR, Currency.DKK)]
        [DataRow(Currency.DKK, Currency.DKK, Currency.DKK)]
        public async Task TestCorrectExchangeWhenNoCache(Currency baseCurrency, Currency mainCurrency, Currency moneyCurrency)
        {
            var settings = Options.Create(new FreeCurrencyApiOptions
            {
                BaseCurrency = baseCurrency,
            });
            TestCorrectExchange(mainCurrency, moneyCurrency, settings);
        }

        [TestMethod]
        [DataRow(Currency.DKK, Currency.EUR, Currency.USD)]
        [DataRow(Currency.DKK, Currency.DKK, Currency.USD)]
        [DataRow(Currency.DKK, Currency.EUR, Currency.DKK)]
        [DataRow(Currency.DKK, Currency.DKK, Currency.DKK)]
        public async Task TestCorrectExchangeWithCache(Currency baseCurrency, Currency mainCurrency, Currency moneyCurrency)
        {
            var settings = Options.Create(new FreeCurrencyApiOptions
            {
                BaseCurrency = baseCurrency,
                CacheInSeconds = 60
            });

            // second call should hit the cache
            TestCorrectExchange(mainCurrency, moneyCurrency, settings);
            TestCorrectExchange(mainCurrency, moneyCurrency, settings);
        }

        private static void TestCorrectExchange(Currency mainCurrency, Currency moneyCurrency, IOptions<FreeCurrencyApiOptions> settings)
        {
            var currencyPair = new CurrencyPair(mainCurrency, moneyCurrency);

            var baseCurrency = settings.Value.BaseCurrency;
            var mainRate = baseCurrency == mainCurrency ? 1 : 2.1m;
            var moneyRate = baseCurrency == moneyCurrency ? 1 : 3.4m;

            var httpClient = CreateMockHttpClient($@"{{
                ""data"": {{
                    ""{mainCurrency}"" : {mainRate},
                    ""{moneyCurrency}"" : {moneyRate}
                }}
            }}");
            
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var provider = new FreeCurrencyApiProvider(httpClient, settings, memoryCache);

            var rate = provider.GetExchangeRate(currencyPair, CancellationToken.None)
                .ShouldNotThrow();

            rate.CurrencyPair.ShouldBe(currencyPair);
            rate.Rate.ShouldBe(moneyRate / (decimal)mainRate);
        }

        private static HttpClient CreateMockHttpClient(string payload)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(payload)
               })
               .Verifiable();

            return new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com")
            };
        }
    }
}
