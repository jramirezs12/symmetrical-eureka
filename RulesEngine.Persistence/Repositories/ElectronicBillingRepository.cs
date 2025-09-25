using MongoDB.Driver;
using RulesEngine.Domain.ElectronicBillingRuleEngine.Entities;
using RulesEngine.Domain.ElectronicBillingRuleEngine.Repository;
using RulesEngine.Persistence.Abstractions;
using RulesEngine.Persistence.Configurations.DataBaseConfiguration;

namespace RulesEngine.Persistence.Repositories
{
    public class ElectronicBillingRepository : MongoRepository<ElectronicBillingEntity, string>, IElectronicBillingRepository
    {
        public ElectronicBillingRepository(GiproDbContext context, ICurrentTenantService tenantService) 
            : base(context, tenantService.TenantId, "ElectronicBillingRuleEngine")
        { }

        public async Task<ElectronicBillingEntity> GetElectronicBilling(FilterDefinition<ElectronicBillingEntity> filter)
        {
            await _collection.Indexes.CreateOneAsync(new CreateIndexModel<ElectronicBillingEntity>(Builders<ElectronicBillingEntity>.IndexKeys.Ascending(x => x.InvoiceNumber)));
            await _collection.Indexes.CreateOneAsync(new CreateIndexModel<ElectronicBillingEntity>(Builders<ElectronicBillingEntity>.IndexKeys.Ascending(x => x.NitIps)));

            var cursor = await _collection.FindAsync(filter, new FindOptions<ElectronicBillingEntity>());


            return await cursor.FirstOrDefaultAsync();
        }
    }
}
