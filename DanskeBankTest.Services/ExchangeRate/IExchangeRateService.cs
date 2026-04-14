using DanskeBankTest.Services.Types;

namespace DanskeBankTest.Services.ExchangeRate
{
    public interface IExchangeRateService
    {
        ValueTask<CurrencyRate> GetExchangeRate(CurrencyPair currencyPair, CancellationToken ct);
    }
}
