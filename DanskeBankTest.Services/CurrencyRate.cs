using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services
{
    public record CurrencyRate(Currency MainCurrency, Currency MoneyCurrency, decimal Rate);
}
