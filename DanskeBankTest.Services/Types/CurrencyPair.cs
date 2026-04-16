using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services.Types
{
    public record CurrencyPair(Currency MainCurrency, Currency MoneyCurrency)
    {
        public bool IsSameCurrency => MainCurrency == MoneyCurrency;
    };
}
