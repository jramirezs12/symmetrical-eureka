using RulesEngine.Domain.ClaimsQueue.Entities;
using RulesEngine.Domain.ClaimsQueue.Repository;
using RulesEngine.Persistence.Abstractions;
using RulesEngine.Persistence.Configurations.DataBaseConfiguration;

namespace RulesEngine.Persistence.Repositories
{
    public class ClaimsQueueRepository : MongoRepository<ClaimsQueueEntity, string>, IClaimsQueueRepository
    {
        public ClaimsQueueRepository(GiproDbContext context, ICurrentTenantService tenantService) : base(context,
                                                                                                    tenantService.TenantId, "ClaimsQueue")
        { }
    }
}
