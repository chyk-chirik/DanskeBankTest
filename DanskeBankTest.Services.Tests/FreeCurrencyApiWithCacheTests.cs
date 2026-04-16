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
using ZiggyCreatures.Caching.Fusion;

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
        public async Task TestCorrectExchange(Currency baseCurrency, Currency mainCurrency, Currency moneyCurrency)
        {
            var currencyPair = new CurrencyPair(mainCurrency, moneyCurrency);

            var mainRate = baseCurrency == mainCurrency ? 1 : 2.1m;
            var moneyRate = baseCurrency == moneyCurrency ? 1 : 3.4m;

            var httpClient = CreateMockHttpClient($@"{{
                ""data"": {{
                    ""{mainCurrency}"" : {mainRate},
                    ""{moneyCurrency}"" : {moneyRate}
                }}
            }}");

            var settings = Options.Create(new FreeCurrencyApiOptions
            {
                BaseCurrency = baseCurrency,
                CacheInSeconds = 60
            });

            var memoryCache = new FusionCache(new FusionCacheOptions());
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
