using DanskeBankTest.Services.Types;

namespace DanskeBankTest.Services.ExchangeRate
{
    public interface IExchangeRateService
    {
        Task<CurrencyRate> GetExchangeRate(CurrencyPair currencyPair, CancellationToken ct);
    }
}
