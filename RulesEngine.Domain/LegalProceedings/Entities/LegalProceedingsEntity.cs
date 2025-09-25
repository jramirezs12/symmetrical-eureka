using MongoDB.Bson.Serialization.Attributes;
using RulesEngine.Domain.Primitives;

namespace RulesEngine.Domain.LegalProceedings.Entities
{
    public class LegalProceedingsEntity : Entity<string>
    {
        [BsonElement("ProcessNumber")]
        public string ProcessNumber { get; set; } = string.Empty;
        [BsonElement("ClaimantId")]
        public string ClaimantId { get; set; } = string.Empty;
        [BsonElement("InvoiceProcess")]
        public string InvoiceProcess { get; set; } = string.Empty;
        [BsonElement("ClaimValue")]
        public int ClaimValue { get; set; }
    }
}
