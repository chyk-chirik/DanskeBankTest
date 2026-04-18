using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.FreeCurrencyApiClient
{
    public static class DependencyInjection
    {
        public static void ConfigureFreeCurrencyApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FreeCurrencyApiOptions>(configuration.GetSection("FreeCurrencyApiOptions"));
            services.AddHttpClient<IFreeCurrencyApiClient, FreeCurrencyApiClient>((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<FreeCurrencyApiOptions>>().Value;

                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
            }).AddHttpMessageHandler<FreeCurrencyApiKeyHandler>(); // with current fusion cache setting we can avoid jitter/curcuit breaks/retries

            services.AddTransient<FreeCurrencyApiKeyHandler>();
        }
    }
}
