using RulesEngine.Domain.LegalProceedings.Entities;
using RulesEngine.Domain.LegalProceedings.Repository;
using RulesEngine.Persistence.Abstractions;
using RulesEngine.Persistence.Configurations.DataBaseConfiguration;

namespace RulesEngine.Persistence.Repositories
{
    public class LegalProceedingsRepository : MongoRepository<LegalProceedingsEntity, string>, ILegalProceedingsRepository
    {
        public LegalProceedingsRepository(GiproDbContext context, ICurrentTenantService tenantService)
            : base(context, tenantService.TenantId, "LegalProceedings")
        { }
    }
}
