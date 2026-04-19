namespace DanskeBankTest.Services.Types
{
    public record CurrencyPair(Currency MainCurrency, Currency MoneyCurrency)
    {
        public bool IsSameCurrency => MainCurrency == MoneyCurrency;
    };
}
