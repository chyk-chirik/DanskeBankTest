using DanskeBankTest.Services;
using DanskeBankTest.Services.ExchangeRate;
using DanskeBankTest.Services.ExchangeRate.FreeCurrencyApi;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using System.Runtime;

namespace DanskeBankTest.Run
{
    internal static class ConfigureServices
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureFreeCurrencyApi(configuration);

            services.AddFusionCache();

            services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
                .MinimumLevel.Information()
                .ReadFrom.Configuration(configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"));
        }

        public static void ConfigureFreeCurrencyApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FreeCurrencyApiOptions>(configuration.GetSection("FreeCurrencyApiOptions"));
            services.AddHttpClient<IExchangeRateService, FreeCurrencyApiProvider>((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<FreeCurrencyApiOptions>>().Value;

                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(6);
            }).AddHttpMessageHandler<FreeCurrencyApiKeyHandler>(); // with current fusion cache setting we can avoid jitter/curcuit breaks/retries

            services.AddTransient<FreeCurrencyApiKeyHandler>();
        }
    }
}
