using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services
{
    public record Money(decimal Amount, Currency Currency);
}
