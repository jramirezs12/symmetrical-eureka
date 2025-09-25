using RulesEngine.Application.DataSources;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Infrastructure.Services;

namespace RulesEngine.Infrastructure.Factories
{
    public class SolidariaDomainModelBuilder(IExternalDataLoader loader) : ITenantDomainModelBuilder
    {
        public string Tenant => "Solidaria";

        public object Build(string radNumber)
        {

            return new InvoiceToCheckSolidaria(radNumber, loader);
        }
    }
}
