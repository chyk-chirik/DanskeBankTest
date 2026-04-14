using DanskeBankTest.Services.Types;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services.Tests
{
    [TestClass]
    public sealed class CurrencyRateTests
    {
        [TestMethod]
        [DataRow(Currency.Eur, Currency.Dkk, Currency.Eur)]
        [DataRow(Currency.Eur, Currency.Dkk, Currency.Dkk)]
        [DataRow(Currency.Eur, Currency.Eur, Currency.Dkk)]
        [DataRow(Currency.Eur, Currency.Eur, Currency.Eur)]
        public void IfCurrencyRateMainCurrencyNotSameAsMoneyCurrency_ExchangeFails(Currency rateMainCurrency, Currency rateMoneyCurrency, Currency moneyCurrency)
        {
            var currencyRate = new CurrencyRate(new CurrencyPair(rateMainCurrency, rateMoneyCurrency), 1.5m);

            var shouldSuccess = rateMainCurrency == moneyCurrency;

            currencyRate.TryConvert(new Money(100m, moneyCurrency), out var result).ShouldBe(shouldSuccess);
        }

    }
}
