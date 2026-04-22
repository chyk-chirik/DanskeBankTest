using DanskeBankTest.Services.ExchangeRate;
using DanskeBankTest.FreeCurrencyApiClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.Extensions.Options;

namespace DanskeBankTest.Run
{
    internal static class ConfigureServices
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureFreeCurrencyApi(configuration, resilienceConfiguration: (opt, sp) =>
            {
                var exchangeRateOpt = sp.GetRequiredService<IOptions<ExchangeRateOptions>>().Value;

                opt.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(exchangeRateOpt.FactoryHardTimeoutInSeconds);
            });

            services.Configure<ExchangeRateOptions>(configuration.GetSection("ExchangeRateOptions"));
            services.AddTransient<IExchangeRateService, ExchangeRateService>();

            services.AddFusionCache();

            services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
                .MinimumLevel.Information()
                .ReadFrom.Configuration(configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}"));
        }
    }
}
