using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.Research.Entities;

namespace RulesEngine.Domain.Research.Repository
{
    public interface IResearchRepository : IMongoRepository<ResearchEntity, string>
    {
    }
}
