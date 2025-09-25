using RulesEngine.Domain.BlobsStorage.Entities;
using RulesEngine.Domain.Invoices.Repositories;

namespace RulesEngine.Domain.BlobsStorage.Repositories
{
    public interface IBlobStorageRepository : IMongoRepository<BlobStorage, string>
    {
    }
}