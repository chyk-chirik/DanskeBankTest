using DanskeBankTest.Services.Types;

namespace DanskeBankTest.Services.ExchangeRate
{
    public class RateData
    {
        public Currency BaseCurrency { get; set; }
        
        public Dictionary<Currency, decimal> Data { get; set; } = null!;
    }
}
