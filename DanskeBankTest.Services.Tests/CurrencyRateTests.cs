using DanskeBankTest.Services.Types;
using Shouldly;

namespace DanskeBankTest.Services.Tests
{
    [TestClass]
    public sealed class CurrencyRateTests
    {
        [TestMethod]
        [DataRow(Currency.EUR, Currency.DKK, Currency.EUR)]
        [DataRow(Currency.EUR, Currency.DKK, Currency.DKK)]
        [DataRow(Currency.EUR, Currency.EUR, Currency.DKK)]
        [DataRow(Currency.EUR, Currency.EUR, Currency.EUR)]
        public void GetRelativeMoneyRate_2RatesSameBaseCurrency_CorrectRelativeRate(Currency baseCurrency, Currency currency1, Currency currency2)
        {
            // good point to rethink: if somebody tries to initialize a CurrencyRate with same main and money currency, should we throw an exception or just allow it? 
            var rate1 = new CurrencyRate(new CurrencyPair(baseCurrency, currency1), 1.5m);
            var rate2 = new CurrencyRate(new CurrencyPair(baseCurrency, currency2), 2.5m);

            CurrencyRate.GetRelativeMoneyRate(rate1, rate2).ShouldBe(new CurrencyRate(new CurrencyPair(currency1, currency2), rate2.Rate / rate1.Rate));
        }

        [TestMethod]
        [DataRow(Currency.EUR, Currency.DKK, Currency.EUR)]
        [DataRow(Currency.EUR, Currency.DKK, Currency.DKK)]
        [DataRow(Currency.EUR, Currency.EUR, Currency.DKK)]
        [DataRow(Currency.EUR, Currency.EUR, Currency.EUR)]
        public void TryExchange_MainCurrencyNotSameAsMoneyCurrency_ExchangeFails(Currency rateMainCurrency, Currency rateMoneyCurrency, Currency moneyCurrency)
        {
            var currencyRate = new CurrencyRate(new CurrencyPair(rateMainCurrency, rateMoneyCurrency), 1.5m);

            var shouldSuccess = rateMainCurrency == moneyCurrency;

            currencyRate.TryExchange(new Money(100m, moneyCurrency), out var result).ShouldBe(shouldSuccess);
        }

        [TestMethod]
        public void TryExchange_MoneyAsLeftOrRightOperand_MoneyExchangedCorrectly()
        {
            var currencyRate = new CurrencyRate(new CurrencyPair(Currency.EUR, Currency.DKK), 7.47m);
            var money = new Money(100m, currencyRate.CurrencyPair.MainCurrency);

            var exchangeResultMoneyLeftOperand = money * currencyRate;
            var exchangeResultMoneyRightOperand = currencyRate * money;
            currencyRate.TryExchange(money, out var exchangeResultTryConvert);

            exchangeResultMoneyLeftOperand.ShouldBe(exchangeResultMoneyRightOperand);
            exchangeResultMoneyLeftOperand.ShouldBe(exchangeResultTryConvert);

            exchangeResultMoneyLeftOperand.Amount.ShouldBe(currencyRate.Rate * money.Amount);
        }

        [TestMethod]
        public void MultiplyMoneyOnConversionRate_SameCurrencyForConversionRateNoMathMustBePerfomedAndMoneyShouldNotBeChanged()
        {
            var currencyRate = new CurrencyRate(new CurrencyPair(Currency.EUR, Currency.EUR), 2); // indicator if calculation was involved
            var money = new Money(100m, currencyRate.CurrencyPair.MainCurrency);

            var exchangeResult = money * currencyRate;
            exchangeResult.ShouldBe(money);
        }
    }
}
