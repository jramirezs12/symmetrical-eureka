using RulesEngine.Application.Common.Enumerations;
using RulesEngine.Domain.BlobsStorage.Entities;

namespace RulesEngine.Infrastructure.Extensions
{
    public static class BlobStorageExtensions
    {
        public static SharedResource FindResource(BlobStorage blobStorage, InputSources source)
        {
            if (blobStorage?.SharedResources == null || !blobStorage.SharedResources.Any())
                throw new Exception("No hay recursos compartidos cargados en BlobStorage.");

            return blobStorage.SharedResources
                .First(x => x._id == (int)source);
        }
    }
}
