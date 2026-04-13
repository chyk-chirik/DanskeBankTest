using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services
{
    public interface IExchangeService
    {
        // in practice I guess more overloads would be needed, but for the sake of this test, let's keep it simple
        ValueTask<Money> Exchange(ExchangeRequest request, CancellationToken ct);
    }

    public class ExchangeService : IExchangeService
    {
        public async ValueTask<Money> Exchange(ExchangeRequest request, CancellationToken ct)
        {
            return null;
        }
    }
}
