using RulesEngine.Application.DataSources;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Infrastructure.Services;

namespace RulesEngine.Infrastructure.Factories
{
    public class MundialDomainModelBuilder(IExternalDataLoader loader) : ITenantDomainModelBuilder
    {
        public string Tenant => "Mundial";

        public object Build(string radNumber)
        {

            return new InvoiceToCheck(radNumber, loader);
        }
    }
}
