using AutoMapper;
using RulesEngine.Application.Invoices.UpdateInvoice;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;
using RulesEngineAPI.Contracts.Solidaria;

namespace RulesEngine.Application.Clients.Solidaria.Automapper
{
    public class SolidariaProfile : Profile
    {
        public SolidariaProfile()
        {
            CreateMap<string, Date>().ConvertUsing<StringToDateConverter>();
            CreateMap<string, Currency>().ConvertUsing<StringToCurrencyConverter>();
            CreateMap<InvoiceToCheckRequest, UpdateInvoiceCommand>();
            CreateMap<UpdateInvoiceCommand, InvoiceToCheckSolidaria>();
        }
    }
}
