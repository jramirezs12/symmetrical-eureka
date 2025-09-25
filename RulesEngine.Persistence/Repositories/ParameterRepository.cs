using RulesEngine.Domain.Parameters.Entities;
using RulesEngine.Domain.Parameters.Repositories;
using RulesEngine.Persistence.Abstractions;
using RulesEngine.Persistence.Configurations.DataBaseConfiguration;

namespace RulesEngine.Persistence.Repositories
{
    public class ParameterRepository : MongoRepository<Parameter , string>, IParameterRepository
    {
        public ParameterRepository(BasicGiproDbContext context, ICurrentTenantService tenantService) : base(context, tenantService.BasicTenantId,"Parameters")
        {
            
        }
    }
}
