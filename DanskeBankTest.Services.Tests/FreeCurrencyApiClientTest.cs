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

    public sealed class FreeCurrencyApiClientTest
    {
        [TestMethod]
        public async Task GetRates_SuccessApiResponse_CorrectRates()
        {
            var httpClient = CreateMockHttpClient("""
                {
                  "data": {
                    "DKK": 1,
                    "EUR": 2
                  }
                }
                """);

            var memoryCache = new FusionCache(new FusionCacheOptions());
            var logger = NullLogger<FreeCurrencyApiClient.FreeCurrencyApiClient>.Instance;
            var client = new FreeCurrencyApiClient.FreeCurrencyApiClient(httpClient, logger);

            var rates = client.GetRates("base currency", ["some currency"], CancellationToken.None)
                .ShouldNotThrow();

            rates.Response!["DKK"].ShouldBe(1);
            rates.Response!["EUR"].ShouldBe(2);
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
