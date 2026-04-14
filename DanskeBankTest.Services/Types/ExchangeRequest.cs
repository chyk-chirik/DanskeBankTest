using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services.Types
{
    public record ExchangeRequest(CurrencyPair CurrencyPair, decimal Amount)
    {
        // would like to have explicit method with a meangful name instead of implicit/explicit operator conversion to Money
        public Money GetOriginalMoney()
        {
            return new Money(Amount, CurrencyPair.MainCurrency);
        }
    }
}
