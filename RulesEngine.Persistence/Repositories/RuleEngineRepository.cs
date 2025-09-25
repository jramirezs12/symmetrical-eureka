using MongoDB.Driver;
using RulesEngine.Domain.RulesEngine.Entities;
using RulesEngine.Domain.RulesEngine.Repositories;
using RulesEngine.Persistence.Abstractions;
using RulesEngine.Persistence.Configurations.DataBaseConfiguration;

namespace RulesEngine.Persistence.Repositories;

public class RuleEngineRepository : MongoRepository<RuleEngine, string>, IRuleEngineRepository
{
    public RuleEngineRepository(BasicGiproDbContext context, ICurrentTenantService tenantService) : base(context, tenantService.BasicTenantId, "RulesEngine")
    { }

    public async Task<RuleEngine> GetRulesCollectionByStage(string stage, string clientName)
    {
        var filterBuilder = Builders<RuleEngine>.Filter.Eq(x => x.Tenant, clientName) &
                            Builders<RuleEngine>.Filter.Eq(x => x.Code, stage);

        var ruleEngines = await _collection.FindAsync(filterBuilder);
        var ruleEngine = ruleEngines.FirstOrDefault();

        if (ruleEngine != null && ruleEngine.Rules != null)
        {
            ruleEngine.Rules = ruleEngine.Rules.Where(x => x.Active == true).ToList();
        }
        
        return ruleEngine!;
    }
}