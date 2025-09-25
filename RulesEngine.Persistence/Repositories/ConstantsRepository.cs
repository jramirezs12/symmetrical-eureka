using RulesEngine.Domain.Constants;
using RulesEngine.Domain.Constants.Entities;
using RulesEngine.Persistence.Abstractions;
using RulesEngine.Persistence.Configurations.DataBaseConfiguration;

namespace RulesEngine.Persistence.Repositories
{
    public class ConstantsRepository : MongoRepository<ConstantsEntity, string>, IConstantsRepository
    {
        public ConstantsRepository(BasicGiproDbContext context, ICurrentTenantService tenantService) : base(context, 
                                                                                                    tenantService.BasicTenantId, "Constants")
        {}
    }
}
