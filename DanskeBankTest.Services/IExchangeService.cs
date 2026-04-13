using DanskeBankTest.Services.ExchangeRate;
using DanskeBankTest.Services.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services
{
    public record ExchangeRequest(Currency MainCurrency, Currency MoneyCurrency, decimal Amount);
    public interface IExchangeService
    {
        // in practice I guess more overloads would be needed, but for the sake of this test, let's keep it simple
        ValueTask<Money> Exchange(ExchangeRequest request, CancellationToken ct);
    }

    public class ExchangeService(
        [FromKeyedServices(KeyedServicesNames.ExchangeRateRealtime)] IEnumerable<IExchangeRateService> exchangeRateServices) : IExchangeService
    {
        public async ValueTask<Money> Exchange(ExchangeRequest request, CancellationToken ct)
        {
            return null;
        }
    }
}
