using RulesEngine.Domain.DisputeProcess.Entities;
using RulesEngine.Domain.DisputeProcess.Repository;
using RulesEngine.Persistence.Abstractions;
using RulesEngine.Persistence.Configurations.DataBaseConfiguration;

namespace RulesEngine.Persistence.Repositories
{
    public class LegalProcessesAndTransactionContractsRepository : MongoRepository<DisputeProcessEntity, string>, ILegalProcessesAndTransactionContractsRepository
    {
        public LegalProcessesAndTransactionContractsRepository(GiproDbContext context, ICurrentTenantService tenantService)
            : base(context, tenantService.TenantId, "LegalProcessesAndTransactionContractsParameters")
        {
        }
    }
}
