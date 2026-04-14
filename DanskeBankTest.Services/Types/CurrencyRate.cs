using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services.Types
{
    public record CurrencyRate(CurrencyPair CurrencyPair, decimal Rate)
    {
        public Money Convert(Money money)
        {
            if (money.Currency != CurrencyPair.MoneyCurrency)
                throw new InvalidOperationException($"Cannot convert money with currency {money.Currency} using this rate, because it is for currency pair {CurrencyPair}.");
            
            return new Money(money.Amount * Rate, CurrencyPair.MainCurrency);
        }
    }
}
