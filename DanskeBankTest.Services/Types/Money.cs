namespace DanskeBankTest.Services.Types
{
    // more handy overloads could be added, but I think these are the most useful ones for the current requirements
    public record Money(decimal Amount, Currency Currency)
    {
        public static Money operator *(Money money, CurrencyRate currencyRate)
        {
            if(currencyRate.TryExchange(money, out var result))
            { 
                return result;
            }

            throw new InvalidOperationException($"Cannot convert {money} using {currencyRate}");
        }

        public static Money operator *(CurrencyRate currencyRate, Money money)
        {
            return money * currencyRate;
        }

        public override string ToString()
        {
            return $"{Amount:F3} {Currency}";
        }
    }
}
