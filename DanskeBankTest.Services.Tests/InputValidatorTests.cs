using Shouldly;

namespace DanskeBankTest.Services.Tests
{
    [TestClass]
    public sealed class InputValidatorTests
    {
        [TestMethod]
        public void NoArguments_ReturnFalse()
        {
            InputValidator.ValidateConsoleArguments(Array.Empty<string>(), out var exchange, out var errorMessage)
                .ShouldBeFalse();
        }

        [TestMethod]
        public void MoreThenTwoArguments_ReturnFalse()
        {
            InputValidator.ValidateConsoleArguments(["EUR/DKK", "100", "LetsImagineWeHaveStrictPolicyOnInput"], out var exchange, out var errorMessage)
                .ShouldBeFalse();
        }

        [TestMethod]
        [DataRow("DKK/EUR", "-100")]
        [DataRow("DKK/EUR", "-100.21.2")]
        [DataRow("DKK/EUR", " ")]
        [DataRow("DKK/EUR", "x")]
        [DataRow("CAD/EUR", "100")]
        [DataRow("EURR/USD", "100")]
        [DataRow("USD//EUR", "100")]
        [DataRow("USD/EUR/", "100")]
        [DataRow("DKK ", "100")]
        [DataRow("DKK/ ", "100")]
        public void NotValidArguments_ReturnFalse(string currencyPair, string amount)
        {
            InputValidator.ValidateConsoleArguments([ currencyPair, amount ], out var exchange, out var errorMessage)
                .ShouldBeFalse();
        }

        [TestMethod]
        [DataRow("DKK/EUR", "100.31", Currency.Dkk, Currency.Eur)]
        [DataRow("EUR/DKK", "100.31", Currency.Eur, Currency.Dkk)]
        [DataRow("DKK/DKK", "100.31", Currency.Dkk, Currency.Dkk)]
        public void ValidArguments_ReturnTrueAndCorrectlyParsedValues(string currencyPair, string amount, Currency mainCurrency, Currency moneyCurrency)
        {
            InputValidator.ValidateConsoleArguments([currencyPair, amount], out var exchange, out var errorMessage)
                .ShouldBeTrue();

            errorMessage.ShouldBeNull();

            exchange.ShouldNotBeNull();
            exchange.MainCurrency.ShouldBe(mainCurrency);
            exchange.MoneyCurrency.ShouldBe(moneyCurrency);
            exchange.Amount.ShouldBe(decimal.Parse(amount));
        }
    }
}
