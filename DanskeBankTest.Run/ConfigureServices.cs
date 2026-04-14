using DanskeBankTest.Services;
using DanskeBankTest.Services.ExchangeRate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DanskeBankTest.Run
{
    internal static class ConfigureServices
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddKeyedTransient<IExchangeRateService, ExchangeRateApiProvider>(KeyedServicesNames.ExchangeRateRealtime);
            services.AddKeyedTransient<IExchangeRateService, FreeCurrencyApiProvider>(KeyedServicesNames.ExchangeRateRealtime);

            services.AddTransient<Func<string, IEnumerable<IExchangeRateService>>>(sp => key =>
            {
                return key switch
                {
                    KeyedServicesNames.ExchangeRateRealtime => [
                        sp.GetRequiredKeyedService<IExchangeRateService>(KeyedServicesNames.ExchangeRateRealtime)],
                    _ => Enumerable.Empty<IExchangeRateService>()
                };
            });
        }
    }
}
