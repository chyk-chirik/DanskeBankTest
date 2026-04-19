using System.Text.Json;

namespace DanskeBankTest.FreeCurrencyApiClient
{
    public class FreeCurrenceApiResponse<T>
    {
        public FreeCurrenceApiResponse()
        {
        }

        public FreeCurrenceApiResponse(T? response, bool isSuccess, int statusCode, string? errorMessage = null)
        {
            Response = response;
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }

        public T? Response { get; internal set; }

        public bool IsSuccess { get; internal set; }

        public int StatusCode { get; internal set; }

        public string? ErrorMessage { get; internal set; }

        public static async Task<FreeCurrenceApiResponse<T>> FromResponse(HttpResponseMessage response, JsonSerializerOptions? options = null, CancellationToken ct = default)
        {
            var apiResponse = new FreeCurrenceApiResponse<T>
            {
                IsSuccess = response.IsSuccessStatusCode,
                StatusCode = (int)response.StatusCode
            };

            if (!response.IsSuccessStatusCode)
            {
                apiResponse.ErrorMessage = await response.Content.ReadAsStringAsync(ct);
                return apiResponse;
            }

            using var responseStream = await response.Content.ReadAsStreamAsync(ct);

            apiResponse.Response = await JsonSerializer.DeserializeAsync<T>(responseStream, options, ct);

            return apiResponse;
        }
    }
}
