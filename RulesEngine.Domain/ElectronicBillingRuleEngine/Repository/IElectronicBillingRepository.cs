using MongoDB.Driver;
using RulesEngine.Domain.ElectronicBillingRuleEngine.Entities;
using RulesEngine.Domain.Invoices.Repositories;

namespace RulesEngine.Domain.ElectronicBillingRuleEngine.Repository
{
    public interface IElectronicBillingRepository : IMongoRepository<ElectronicBillingEntity, string>
    {
        Task<ElectronicBillingEntity> GetElectronicBilling(FilterDefinition<ElectronicBillingEntity> filter);
    }
}
