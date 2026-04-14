using DanskeBankTest.Services.Types;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services.Tests
{
    [TestClass]
    public sealed class ExchangeRequestTests
    {
        [TestMethod]
        public void ConvertExchangeRequestToOrinalMoney_MainCurrencyMustBeUsedAsAmountCurrency()
        {
            var mainCurrency = Currency.Usd;
            var amount = 100m;

            var exchangeRequest = new ExchangeRequest(new CurrencyPair(mainCurrency, Currency.Eur), amount);
            var originalSum = exchangeRequest.GetOriginalMoney();

            originalSum.ShouldBe(new Money(amount, mainCurrency));
        }
    }
}
