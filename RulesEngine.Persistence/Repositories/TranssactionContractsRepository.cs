using RulesEngine.Domain.TransactionContracts.Entities;
using RulesEngine.Persistence.Abstractions;
using RulesEngine.Persistence.Configurations.DataBaseConfiguration;

namespace RulesEngine.Persistence.Repositories
{
    internal class TranssactionContractsRepository : MongoRepository<ConsolidatedTransactionContracts, string>, ITransactionContractsRepository
    {
        public TranssactionContractsRepository(GiproDbContext context, ICurrentTenantService tenantService) :
                                base(context, tenantService.TenantId, "ConsolidatedTransactionContracts")
        { }
    }
}
