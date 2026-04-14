
using DanskeBankTest.Run;
using DanskeBankTest.Services;
using DanskeBankTest.Services.ExchangeRate;
using DanskeBankTest.Services.Types;
using Microsoft.Extensions.DependencyInjection;
args = [
    "EUR/USD",
    "100"
];
if (!InputValidator.ValidateConsoleArguments(args, out var exchangeRequest, out var errorMessage))
{
    Console.WriteLine(errorMessage);
    return;
}

var serviceCollection = new ServiceCollection();
serviceCollection.Configure(null!);

using var sp = serviceCollection.BuildServiceProvider();

var exchangeRateService = sp.GetRequiredService<IExchangeRateService>();
var exchangeRate = await exchangeRateService.GetExchangeRate(exchangeRequest!.CurrencyPair, CancellationToken.None);
exchangeRate.TryConvert(exchangeRequest.GetOriginalMoney(), out var exchangedMoney);

Console.WriteLine(exchangedMoney);