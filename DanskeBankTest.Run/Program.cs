
using DanskeBankTest.Run;
using DanskeBankTest.Services;
using DanskeBankTest.Services.ExchangeRate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var serviceCollection = new ServiceCollection();
serviceCollection.Configure(configuration);

using var sp = serviceCollection.BuildServiceProvider();

while (true)
{
    if (!InputValidator.ValidateConsoleArguments(args, out var exchangeRequest, out var errorMessage))
    {
        Console.WriteLine(errorMessage);
    }
    else
    {
        using var scopedSp = sp.CreateScope();
        var exchangeRateService = scopedSp.ServiceProvider.GetRequiredService<IExchangeRateService>();
        var exchangeRate = await exchangeRateService.GetExchangeRate(exchangeRequest!.CurrencyPair, CancellationToken.None);

        var exchangedMoney = exchangeRate * exchangeRequest.GetOriginalMoney();

        Console.WriteLine(exchangedMoney);
    }

     args = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}



