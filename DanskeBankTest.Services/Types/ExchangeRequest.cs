using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services.Types
{
    public record ExchangeRequest(CurrencyPair CurrencyPair, decimal Amount)
    {
        public Money GetOriginalMoney()
        {
            return new Money(Amount, CurrencyPair.MainCurrency);
        }
    }
}
