using MongoDB.Bson;
using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.RulesEngine.Entities;

namespace RulesEngine.Domain.RulesEngine.Repositories
{
    public interface IRuleEngineRepository : IMongoRepository<RuleEngine, string>
    {
        Task<RuleEngine> GetRulesCollectionByStage(string stage, string clientName);
    }
}
