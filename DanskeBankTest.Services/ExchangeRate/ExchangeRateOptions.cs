using DanskeBankTest.Services.Types;

namespace DanskeBankTest.Services.ExchangeRate
{
    public class ExchangeRateOptions
    {
        public Currency BaseCurrency { get; set; } = Currency.DKK;
        public int CacheDurationInSeconds { get; set; }
        public int FailSafeMaxDurationInMinutes { get; set; } = 5;
        public int FailSafeThrottleDurationInSeconds { get; set; } = 2;
        public int FactorySoftTimeoutInMilliseconds { get; set; } = 200;
        public int FactoryHardTimeoutInSeconds { get; set; } = 10;
    }
}
