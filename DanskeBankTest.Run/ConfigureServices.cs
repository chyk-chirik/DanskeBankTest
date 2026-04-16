using DanskeBankTest.Services;
using DanskeBankTest.Services.ExchangeRate;
using DanskeBankTest.Services.ExchangeRate.FreeCurrencyApi;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Runtime;

namespace DanskeBankTest.Run
{
    internal static class ConfigureServices
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureFreeCurrencyApi(configuration);

            services.AddMemoryCache();
        }

        public static void ConfigureFreeCurrencyApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FreeCurrencyApiOptions>(configuration.GetSection("FreeCurrencyApiOptions"));
            services.AddHttpClient<IExchangeRateService, FreeCurrencyApiProvider>((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<FreeCurrencyApiOptions>>().Value;

                client.BaseAddress = new Uri(settings.BaseUrl);
            }).AddHttpMessageHandler<FreeCurrencyApiKeyHandler>();

            services.AddTransient<FreeCurrencyApiKeyHandler>();
        }
    }
}
