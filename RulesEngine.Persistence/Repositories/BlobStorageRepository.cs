using RulesEngine.Domain.BlobsStorage.Entities;
using RulesEngine.Domain.BlobsStorage.Repositories;
using RulesEngine.Persistence.Abstractions;
using RulesEngine.Persistence.Configurations.DataBaseConfiguration;

namespace RulesEngine.Persistence.Repositories
{
    public class BlobStorageRepository : MongoRepository<BlobStorage, string>, IBlobStorageRepository
    {
        public BlobStorageRepository(BasicGiproDbContext context, ICurrentTenantService tenantService) : base(context,
                                                                                                              tenantService.BasicTenantId,
                                                                                                              "BlobsStorage")
        {
        }
    }
}