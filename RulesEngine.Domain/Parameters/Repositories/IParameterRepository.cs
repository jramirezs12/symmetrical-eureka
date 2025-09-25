using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.Parameters.Entities;

namespace RulesEngine.Domain.Parameters.Repositories
{
    public interface IParameterRepository : IMongoRepository<Parameter, string>
    {
    }
}
