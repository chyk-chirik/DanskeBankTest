using DanskeBankTest.Services.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services
{
    public static class InputValidator
    {
        public static bool ValidateConsoleArguments(string[] args, out ExchangeRequest? exchangeRequest, out string? errorMessage)
        {
            exchangeRequest = default;
            errorMessage = null;

            if (args == null || args.Length != 2)
            {
                errorMessage = "Usage: Exchange <currency pair> <amount>";
                return false;
            }

            var currenciesArg = args[0].Split('/');
            var amountArg = args[1];

            if (currenciesArg.Length != 2)
            {
                errorMessage = "Invalid <currency pair> format. Use <currency1>/<currency2>.";
                return false;
            }

            if (!Enum.TryParse<Currency>(currenciesArg[0], true, out var mainCurrency))
            {
                errorMessage = $"Main currency: {currenciesArg[0]} is not supported";
                return false;
            }

            if (!Enum.TryParse<Currency>(currenciesArg[1], true, out var moneyCurrency))
            {
                errorMessage = $"Money currency: {currenciesArg[1]} is not supported";
                return false;
            }

            if (!decimal.TryParse(amountArg, out var amount) || amount < 0)
            {
                errorMessage = "Invalid <amount> format.";
                return false;
            }

            exchangeRequest = new ExchangeRequest(new CurrencyPair(mainCurrency, moneyCurrency), amount);
            return true;
        }
    }
}
