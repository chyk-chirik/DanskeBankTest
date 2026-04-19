using Microsoft.Extensions.Logging;
using System.Net;

namespace DanskeBankTest.FreeCurrencyApiClient
{
    public static partial class Log
    {
        [LoggerMessage(
            EventId = 1,
            Level = LogLevel.Debug,
            Message = "Received response status {Status} for base currency {BaseCurrency}")]
        public static partial void LogFreeCurrencyResponse(ILogger logger, HttpStatusCode status, string baseCurrency);
    }
}
