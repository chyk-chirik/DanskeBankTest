using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Polly;

namespace DanskeBankTest.FreeCurrencyApiClient
{
    public static class DependencyInjection
    {
        public static IHttpClientBuilder ConfigureFreeCurrencyApi(
            this IServiceCollection services, 
            IConfiguration configuration, 
            string sectionName = "FreeCurrencyApiOptions")
        {
            services.AddTransient<FreeCurrencyApiKeyHandler>();

            services.Configure<FreeCurrencyApiOptions>(configuration.GetSection(sectionName));
            return services.AddHttpClient<IFreeCurrencyApiClient, FreeCurrencyApiClient>((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<FreeCurrencyApiOptions>>().Value;

                client.BaseAddress = new Uri(settings.BaseUrl);
            })
            .AddHttpMessageHandler<FreeCurrencyApiKeyHandler>();
        }
    }
}
