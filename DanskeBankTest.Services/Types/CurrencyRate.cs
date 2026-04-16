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

            if(CurrencyPair.IsSameCurrency)
            {
                result = money;
            }
            else
            {
                result = new Money(money.Amount * Rate, CurrencyPair.MoneyCurrency);
            }

            return true;
        }

        public static CurrencyRate GetRelativeMoneyRate(CurrencyRate newMain, CurrencyRate newMoney)
        {
            if (newMain.CurrencyPair.MainCurrency != newMoney.CurrencyPair.MainCurrency)
            {
                throw new ArgumentException("Main currency must be the same for both rates.");
            }

            var relativeRate = newMoney.Rate / newMain.Rate;

            return new CurrencyRate(new CurrencyPair(newMain.CurrencyPair.MoneyCurrency, newMoney.CurrencyPair.MoneyCurrency), relativeRate);
        }
    }
}
