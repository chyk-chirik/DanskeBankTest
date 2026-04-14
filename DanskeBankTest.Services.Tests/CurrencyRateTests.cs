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

            currencyRate.TryExchange(new Money(100m, moneyCurrency), out var result).ShouldBe(shouldSuccess);
        }

        [TestMethod]
        public void ExchangeMoney_MoneyExchangedCorrectly()
        {
            var currencyRate = new CurrencyRate(new CurrencyPair(Currency.Eur, Currency.Dkk), 7.47m);
            var money = new Money(100m, currencyRate.CurrencyPair.MainCurrency);

            var exchangeResultMoneyLeftOperand = money * currencyRate;
            var exchangeResultMoneyRightOperand = currencyRate * money;
            currencyRate.TryExchange(money, out var exchangeResultTryConvert);

            exchangeResultMoneyLeftOperand.ShouldBe(exchangeResultMoneyRightOperand);
            exchangeResultMoneyLeftOperand.ShouldBe(exchangeResultTryConvert);

            exchangeResultMoneyLeftOperand.Amount.ShouldBe(currencyRate.Rate * money.Amount);
        }

        [TestMethod]
        public void ExchangeMoneyWhenMainCurrencyMatchesMoneyCurrency_NoMathMustBePerfomedAndMoneyShouldNotBeChanged()
        {
            var currencyRate = new CurrencyRate(new CurrencyPair(Currency.Eur, Currency.Eur), 2); // indicator if calculation was involved
            var money = new Money(100m, currencyRate.CurrencyPair.MainCurrency);

            var exchangeResult = money * currencyRate;
            exchangeResult.ShouldBe(money);
        }
    }
}
