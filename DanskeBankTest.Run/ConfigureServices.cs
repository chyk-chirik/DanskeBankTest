using DanskeBankTest.Services.ExchangeRate;
using DanskeBankTest.FreeCurrencyApiClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace DanskeBankTest.Run
{
    internal static class ConfigureServices
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureFreeCurrencyApi(configuration)
            .AddStandardResilienceHandler()
            .Configure((opt, sp) => 
            {
                var settings = sp.GetRequiredService<IOptions<ExchangeRateOptions>>().Value;

                opt.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(settings.FactoryHardTimeoutInSeconds);
                opt.Retry.BackoffType = DelayBackoffType.Exponential;
                opt.Retry.UseJitter = true;
                opt.Retry.Delay = TimeSpan.FromMilliseconds(100);
                opt.AttemptTimeout.Timeout = TimeSpan.FromSeconds(2);
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
