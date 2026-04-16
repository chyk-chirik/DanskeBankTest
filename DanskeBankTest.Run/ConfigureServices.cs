using DanskeBankTest.Services;
using DanskeBankTest.Services.ExchangeRate;
using DanskeBankTest.Services.ExchangeRate.FreeCurrencyApi;
using DanskeBankTest.Services.Network;
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
            var mode = configuration.GetValue<ApplicationMode>("ApplicationMode");

            if(mode == ApplicationMode.Production)
            {
                services.ConfigureFreeCurrencyApi(configuration);
            }
            else
            {
                services.AddTransient<IExchangeRateService, OfflineRateProvider>();
            }

            services.AddMemoryCache();
        }

        public static void ConfigureFreeCurrencyApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FreeCurrencyApiOptions>(configuration.GetSection("FreeCurrencyApiOptions"));
            services.AddHttpClient<IExchangeRateService, FreeCurrencyApiProvider>((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<FreeCurrencyApiOptions>>().Value;

                client.BaseAddress = new Uri(settings.BaseUrl);
            }).AddHttpMessageHandler<ApiKeyHandler>();

            services.AddTransient<ApiKeyHandler>();
        }
    }
}
