using NRules.Fluent.Dsl;
using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.Dependencies
{
    public class AutofacDeathDateYearAgoRule_50 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;
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
