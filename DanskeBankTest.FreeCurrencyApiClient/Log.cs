using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;

namespace DanskeBankTest.FreeCurrencyApiClient
{
    public static partial class Log
    {
        [LoggerMessage(
            EventId = 1,
            Level = LogLevel.Debug,
            Message = "Received response status {Status} from Free Currency Api provider for currency {BaseCurrency}")]
        public static partial void LogFreeCurrencyResponse(ILogger logger, HttpStatusCode status, string baseCurrency);
    }
}
