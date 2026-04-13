namespace DanskeBankTest.Services
{
    public interface IExchangeRateService
    {
        // expected to be used on a hot path, so we want to avoid allocations and async state machines if possible
        ValueTask<decimal> GetExchangeRate(Currency from, Currency to);
    }
}
