using AutoMapper;
using RulesEngine.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Application.Clients
{
    public class StringToCurrencyConverter : ITypeConverter<string, Currency>
    {
        public Currency Convert(string src, Currency dest, ResolutionContext ctx)
        {
            return Currency.Create(src);
        }
    }
}
