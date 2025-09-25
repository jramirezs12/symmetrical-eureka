using AutoMapper;
using RulesEngine.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Application.Clients
{
    public class StringToDateConverter : ITypeConverter<string, Date>
    {
        public Date Convert(string src, Date dest, ResolutionContext ctx)
        {
            return Date.Create(src);
        }
    }
}
