using DanskeBankTest.Services.ExchangeRate;
using DanskeBankTest.Services.Types;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services.Tests
{
    //[TestClass]

    //public sealed class ExchangeRateServiceTests
    //{
    //    private CurrencyRate SuccessCurrencyRate1 = new CurrencyRate(new CurrencyPair(Currency.Usd, Currency.Eur), 1m);
    //    private CurrencyRate SuccessCurrencyRate2 = new CurrencyRate(new CurrencyPair(Currency.Usd, Currency.Gbp), 2m);
       
    //    private IExchangeRateService alwaysSuccessRateService1;
    //    private IExchangeRateService alwaysSuccessRateService2;
    //    private IExchangeRateService alwaysErrorRateService;

    //    public ExchangeRateServiceTests()
    //    {
    //        var successRateServiceMock1 = new Mock<IExchangeRateService>();
    //        successRateServiceMock1
    //            .Setup(s => s.GetExchangeRate(It.IsAny<CurrencyPair>(), It.IsAny<CancellationToken>()))
    //            .ReturnsAsync(SuccessCurrencyRate1);

    //        var successRateServiceMock2 = new Mock<IExchangeRateService>();
    //        successRateServiceMock2
    //            .Setup(s => s.GetExchangeRate(It.IsAny<CurrencyPair>(), It.IsAny<CancellationToken>()))
    //            .ReturnsAsync(SuccessCurrencyRate2);

    //        var alwaysErrorRateServiceMock = new Mock<IExchangeRateService>();
    //        alwaysErrorRateServiceMock
    //            .Setup(s => s.GetExchangeRate(It.IsAny<CurrencyPair>(), It.IsAny<CancellationToken>()))
    //            .ThrowsAsync(new Exception("Error"));

    //        alwaysSuccessRateService1 = successRateServiceMock1.Object;
    //        alwaysSuccessRateService2 = successRateServiceMock2.Object;
    //        alwaysErrorRateService = alwaysErrorRateServiceMock.Object;
    //    }

    //    [TestMethod]
    //    public async Task RealtimeServiceException_ReturnRateFromOfflineService()
    //    {
    //        var facadeRateService = new CacheExchangeRateProvider(
    //            realtimeRateService: alwaysErrorRateService,
    //            offlineRateService: alwaysSuccessRateService1);

    //        var anyPair = new CurrencyPair(Currency.Usd, Currency.Eur);
    //        var rate = await facadeRateService.GetExchangeRate(anyPair, CancellationToken.None);

    //        rate.ShouldBe(SuccessCurrencyRate1);
    //    }

    //    [TestMethod]
    //    public async Task RealtimeServiceHasPriorityOverOfflineService()
    //    {
    //        var facadeRateService = new CacheExchangeRateProvider(
    //            realtimeRateService: alwaysSuccessRateService1,
    //            offlineRateService: alwaysSuccessRateService2);

    //        var anyPair = new CurrencyPair(Currency.Usd, Currency.Eur);
    //        var rate = await facadeRateService.GetExchangeRate(anyPair, CancellationToken.None);

    //        rate.ShouldBe(SuccessCurrencyRate1);
    //    }

    //    [TestMethod] // we highlight here assumption that inability to get rate is exceptional non business case
    //    public async Task RealTimeAndOfflineServicesThrowExceptin_ThrowException()
    //    {
    //        var facadeRateService = new CacheExchangeRateProvider(
    //            realtimeRateService: alwaysErrorRateService,
    //            offlineRateService: alwaysErrorRateService);

    //        var anyPair = new CurrencyPair(Currency.Usd, Currency.Eur);

    //        Should.Throw<Exception>(async () => await facadeRateService.GetExchangeRate(anyPair, CancellationToken.None));
    //    }
    //}
}
