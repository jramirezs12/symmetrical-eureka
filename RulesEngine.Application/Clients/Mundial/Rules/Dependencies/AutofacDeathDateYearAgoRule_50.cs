using MongoDB.Bson;
using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.Invoices.Entities;
using Autofac;
using NRules.Fluent;
using Autofac.Core;

namespace RulesEngine.Application.Clients.Mundial.Rules.Dependencies
{
    public class AutofacDeathDateYearAgoRule_50 : Rule
    {
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;
            IInvoiceRepository? invoiceRepository = default;

            Dependency()
                .Resolve(() => invoiceRepository);

            When()
                .Match(() => invoiceToCheck!, x => Convert.ToDateTime(x.DeathDate) > Convert.ToDateTime(x.EventDate).AddYears(1));

            Then()
                .Do(w => Console.WriteLine(invoiceRepository!.FindOne(x => x.RadNumber == "CMVIQ034000000001930").NitIps));
        }
    }
}
