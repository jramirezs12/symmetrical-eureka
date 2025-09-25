using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.Provider.Entities;

namespace RulesEngine.Domain.Provider.Repository
{
    public interface IProviderRepository : IMongoRepository<ProviderData ,string>
    {}
}
