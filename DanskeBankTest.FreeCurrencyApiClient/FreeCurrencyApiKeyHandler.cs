using Microsoft.Extensions.Options;
using System.Web;

namespace DanskeBankTest.FreeCurrencyApiClient
{
    internal class FreeCurrencyApiKeyHandler(IOptions<FreeCurrencyApiOptions> settings) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var uriBuilder = new UriBuilder(request.RequestUri!);

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["apikey"] = settings.Value.ApiKey;
            uriBuilder.Query = query.ToString();

            request.RequestUri = uriBuilder.Uri;

            return await base.SendAsync(request, cancellationToken);
        }
    }
}