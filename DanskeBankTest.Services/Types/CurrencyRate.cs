using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services.Types
{
    public record CurrencyRate(CurrencyPair CurrencyPair, decimal Rate)
    {
        public bool TryConvert(Money money, out Money result)
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
    }
}
