using DanskeBankTest.Services.Types;

namespace DanskeBankTest.Services.ExchangeRate
{
    public interface IExchangeRateService
    {
        // expected to be used on a hot path, so we want to avoid allocations and async state machines if possible
        ValueTask<CurrencyRate> GetExchangeRate(Currency mainCurrency, Currency moneyCurrency, CancellationToken ct);
    }
}
