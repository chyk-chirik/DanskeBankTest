using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services
{
    // on the hot path struct can be better, unless you need to copy it many times,
    // but record/class more convenient to use on API level
    public record ExchangeRequest(Currency From, Currency To, decimal Value);
}
