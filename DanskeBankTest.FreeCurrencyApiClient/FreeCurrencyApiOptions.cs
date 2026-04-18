namespace DanskeBankTest.FreeCurrencyApiClient
{
    public class FreeCurrencyApiOptions
    {
        public string BaseUrl { get; set; } = null!;
        public string ApiKey { get; set; } = null!;
        public int TimeoutSeconds { get; set; } = 6;
    }
}
