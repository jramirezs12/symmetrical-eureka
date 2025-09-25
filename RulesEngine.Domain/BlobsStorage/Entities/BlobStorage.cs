using RulesEngine.Domain.Primitives;

namespace RulesEngine.Domain.BlobsStorage.Entities
{
    public class BlobStorage : Entity<string>
    {
        public string Tenant { get; set; } = string.Empty;
        public string BusinessCode { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public List<Dictionary<string, object>> BlobContainerNames { get; set; } = new();
        public List<SharedResource> SharedResources { get; set; } = new ();
    }

    public class SharedResource
    {
        public int _id { get; set; }
        public string Share { get; set; } = string.Empty;
        public string Directory { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}