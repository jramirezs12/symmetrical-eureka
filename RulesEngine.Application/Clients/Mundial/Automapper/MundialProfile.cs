using AutoMapper;
using RulesEngine.Application.Invoices.UpdateInvoice;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.ValueObjects;
using RulesEngineAPI.Contracts.Mundial;

namespace RulesEngine.Application.Clients.Mundial.Automapper
{
    public class MundialProfile : Profile
    {
        public MundialProfile()
        {
            CreateMap<string, Date>().ConvertUsing<StringToDateConverter>();
            CreateMap<string, Currency>().ConvertUsing<StringToCurrencyConverter>();
            CreateMap<InvoiceToCheckRequest, UpdateInvoiceCommand>();

            CreateMap<UpdateInvoiceCommand, InvoiceToCheck>();
        }
    }
}
