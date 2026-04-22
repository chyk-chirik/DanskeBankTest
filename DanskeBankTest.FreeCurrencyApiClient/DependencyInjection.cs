using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Polly;

namespace DanskeBankTest.FreeCurrencyApiClient
{
    public static class DependencyInjection
    {
        public static void ConfigureFreeCurrencyApi(
            this IServiceCollection services, 
            IConfiguration configuration, 
            Action<HttpStandardResilienceOptions, IServiceProvider>? resilienceConfiguration = null,
            string sectionName = "FreeCurrencyApiOptions")
        {
            services.Configure<FreeCurrencyApiOptions>(configuration.GetSection(sectionName));
            services.AddHttpClient<IFreeCurrencyApiClient, FreeCurrencyApiClient>((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<FreeCurrencyApiOptions>>().Value;

                client.BaseAddress = new Uri(settings.BaseUrl);
            })
            .AddHttpMessageHandler<FreeCurrencyApiKeyHandler>()
            .AddStandardResilienceHandler()
            .Configure((opt, sp) =>
            {
                var settings = sp.GetRequiredService<IOptions<FreeCurrencyApiOptions>>().Value;

                opt.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
                opt.Retry.BackoffType = DelayBackoffType.Exponential;
                opt.Retry.UseJitter = true;
                opt.Retry.Delay = TimeSpan.FromMilliseconds(100);
                opt.AttemptTimeout.Timeout = TimeSpan.FromSeconds(2);

                if(resilienceConfiguration is not null)
                {
                    resilienceConfiguration(opt, sp);
                }
            });

            services.AddTransient<FreeCurrencyApiKeyHandler>();
        }
    }
}
