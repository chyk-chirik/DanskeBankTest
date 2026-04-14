using System;
using System.Collections.Generic;
using System.Text;

namespace DanskeBankTest.Services.Types
{
    public record Money(decimal Amount, Currency Currency)
    {
        public override string ToString()
        {
            return $"{Amount} {Currency}";
        }
    }
}
