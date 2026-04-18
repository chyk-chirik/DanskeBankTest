using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;

namespace DanskeBankTest.FreeCurrencyApiClient
{
    public interface IFreeCurrencyApiClient
    {
        Task<FreeCurrenceApiResponse<Dictionary<string, decimal>>> GetRates(string baseCurrency, IEnumerable<string> currencies, CancellationToken ct);
    }

    public class FreeCurrencyApiClient(HttpClient http, ILogger<FreeCurrencyApiClient> logger) : IFreeCurrencyApiClient
    {
        private static JsonSerializerOptions RateSerializerOptions = new JsonSerializerOptions
        {
            Converters = { new RateConverter() }
        };
        public async Task<FreeCurrenceApiResponse<Dictionary<string, decimal>>> GetRates(string baseCurrency, IEnumerable<string> currencies, CancellationToken ct)
        {
            var escapedCurrencies = Uri.EscapeDataString(string.Join(',', currencies));
            using var response = await http.GetAsync($"latest?base_currency={baseCurrency}&currencies={escapedCurrencies}", ct);
            using var responseStream = await response.Content.ReadAsStreamAsync(ct);

            Log.LogFreeCurrencyResponse(logger, response.StatusCode, baseCurrency);

            return await FreeCurrenceApiResponse<Dictionary<string, decimal>>.FromResponse(response, RateSerializerOptions, ct);
        }
    }
}
