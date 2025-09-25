using RulesEngine.Domain.Research.Entities;
using RulesEngine.Domain.Research.Repository;
using RulesEngine.Persistence.Abstractions;
using RulesEngine.Persistence.Configurations.DataBaseConfiguration;

namespace RulesEngine.Persistence.Repositories
{
    public class ResearchRepository : MongoRepository<ResearchEntity, string>, IResearchRepository
    {
        public ResearchRepository(GiproDbContext context, ICurrentTenantService tenantService) : base(context, tenantService.TenantId,
                                                                                                                "Researchs")
        { }
    }
}
