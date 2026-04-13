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
        [DataRow("DKK/EUR", "100")]
        [DataRow("EUR/DKK", "100")]
        [DataRow("DKK/DKK", "100")]
        public void ValidArguments_ReturnTrue(string currencyPair, string amount)
        {
            InputValidator.ValidateConsoleArguments([currencyPair, amount], out var exchange, out var errorMessage)
                .ShouldBeTrue();
        }
    }
}
