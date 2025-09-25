using MongoDB.Bson.Serialization.Attributes;
using RulesEngine.Domain.Primitives;

namespace RulesEngine.Domain.TransactionContracts.Entities
{
    public class ConsolidatedTransactionContracts : Entity<string>
    {
        [BsonElement("ClaimantId")]
        public string ClaimantId { get; set; } = string.Empty;
        [BsonElement("ClaimNumber")]
        public string ClaimNumber { get; set; } = string.Empty;
        [BsonElement("Observation")]
        public string Observation { get; set; } = string.Empty;
    }
}
