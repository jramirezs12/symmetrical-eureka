using RulesEngine.Domain.Provider.Entities;
using RulesEngine.Domain.Provider.Repository;
using RulesEngine.Persistence.Abstractions;
using RulesEngine.Persistence.Configurations.DataBaseConfiguration;

namespace RulesEngine.Persistence.Repositories
{
    public class ProviderRepository : MongoRepository<ProviderData, string>, IProviderRepository
    {
        public ProviderRepository(GiproDbContext context, ICurrentTenantService tenantService) : base(context, tenantService.TenantId,
                                                                                                                "ProvidersRuleEngine")
        {}
    }
}
