
using DanskeBankTest.Run;
using DanskeBankTest.Services;
using DanskeBankTest.Services.ExchangeRate;
using DanskeBankTest.Services.Types;
using Microsoft.Extensions.Configuration;
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

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var serviceCollection = new ServiceCollection();
serviceCollection.Configure(configuration);

using var sp = serviceCollection.BuildServiceProvider();

var exchangeRateService = sp.GetRequiredService<IExchangeRateService>();
var exchangeRate = await exchangeRateService.GetExchangeRate(exchangeRequest!.CurrencyPair, CancellationToken.None);

var exchangedMoney = exchangeRate * exchangeRequest.GetOriginalMoney();

Console.WriteLine(exchangedMoney);