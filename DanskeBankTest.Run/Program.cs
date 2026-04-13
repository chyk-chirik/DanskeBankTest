
using DanskeBankTest.Run;
using DanskeBankTest.Services;
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
var exchangeService = sp.GetRequiredService<IExchangeService>();

var money = await exchangeService.Exchange(exchangeRequest!, CancellationToken.None);

Console.WriteLine(money.Amount);