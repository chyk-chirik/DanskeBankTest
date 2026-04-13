
using DanskeBankTest.Run;
using DanskeBankTest.Services;
using Microsoft.Extensions.DependencyInjection;

if (!InputValidator.ValidateConsoleArguments(args, out var exchangeRequest, out var errorMessage))
{
    Console.WriteLine(errorMessage);
    return;
}

var serviceCollection = new ServiceCollection();
//serviceCollection.Configure();

using var sp = serviceCollection.BuildServiceProvider();
