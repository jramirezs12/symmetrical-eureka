using RulesEngine.Domain.ClaimsQueue.Entities;
using RulesEngine.Domain.Invoices.Repositories;

namespace RulesEngine.Domain.ClaimsQueue.Repository
{
    public interface IClaimsQueueRepository : IMongoRepository<ClaimsQueueEntity, string>
    { }
}
