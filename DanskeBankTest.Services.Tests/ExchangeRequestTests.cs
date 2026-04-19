using DanskeBankTest.Services.Types;
using Shouldly;

namespace DanskeBankTest.Services.Tests
{
    [TestClass]
    public sealed class ExchangeRequestTests
    {
        [TestMethod]
        public void GetOriginalMoney_ExchangeRequest_ExchangeMainCurrencyAndAmountIsMoneyCurrencyAndAmount()
        {
            var mainCurrency = Currency.USD;
            var amount = 100m;

            var exchangeRequest = new ExchangeRequest(new CurrencyPair(mainCurrency, Currency.EUR), amount);
            var originalSum = exchangeRequest.GetOriginalMoney();

            originalSum.ShouldBe(new Money(amount, mainCurrency));
        }
    }
}
