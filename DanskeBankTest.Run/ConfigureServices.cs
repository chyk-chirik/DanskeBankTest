using DanskeBankTest.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Run
{
    internal static class ConfigureServices
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IExchangeService, ExchangeService>();
        }
    }
}
