using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services.Types
{
    public record CurrencyRate(CurrencyPair CurrencyPair, decimal Rate)
    {
        public bool TryExchange(Money money, out Money result)
        {
            if (money.Currency != CurrencyPair.MainCurrency)
            {
                result = null!;
                return false;
            }

            if(CurrencyPair.MainCurrency == CurrencyPair.MoneyCurrency)
            {
                result = new Money(money.Amount, CurrencyPair.MoneyCurrency);
            }
            else
            {
                result = new Money(money.Amount * Rate, CurrencyPair.MoneyCurrency);
            }

            return true;
        }

        public static CurrencyRate FromRelativeRate(CurrencyRate main, CurrencyRate money)
        {
            if (main.CurrencyPair.MainCurrency != money.CurrencyPair.MainCurrency)
            {
                throw new ArgumentException("Main currency must be the same for both rates.");
            }

            var relativeRate = money.Rate / main.Rate;

            return new CurrencyRate(new CurrencyPair(main.CurrencyPair.MoneyCurrency, money.CurrencyPair.MoneyCurrency), relativeRate);
        }
    }
}
