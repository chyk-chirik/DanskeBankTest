
using DanskeBankTest.Services;

if (!InputValidator.ValidateConsoleArguments(args, out var exchangeRequest, out var errorMessage))
{
    Console.WriteLine(errorMessage);
    return;
}


