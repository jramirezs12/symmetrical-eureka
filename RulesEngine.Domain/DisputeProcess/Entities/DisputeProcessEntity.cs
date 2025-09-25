using MongoDB.Bson.Serialization.Attributes;
using RulesEngine.Domain.Primitives;

namespace RulesEngine.Domain.DisputeProcess.Entities
{
    public class DisputeProcessEntity : Entity<string>
    {
        [BsonElement("ClaimantId")]
        public string ClaimantId { get; set; } = string.Empty;
        [BsonElement("InvoiceNumber")]
        public string InvoiceNumber { get; set; } = string.Empty;
        [BsonElement("ProcessNumber")]
        public string ProcessNumber { get; set; } = string.Empty;
        [BsonElement("ClaimValue")]
        public int ClaimValue { get; set; }
        [BsonElement("Observation")]
        public string Observation { get; set; } = string.Empty;
        [BsonElement("UploadDate")]
        public DateTime UploadDate { get; set; }
        [BsonElement("Type")]
        public string Type { get; set; } = string.Empty;
        [BsonElement("Active")]
        public bool Active { get; set; }
    }
}
