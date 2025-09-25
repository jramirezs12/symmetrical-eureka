using RulesEngine.Domain.Constants.Entities;
using RulesEngine.Domain.Invoices.Repositories;

namespace RulesEngine.Domain.Constants
{
    public interface IConstantsRepository : IMongoRepository<ConstantsEntity, string>
    {}
}
