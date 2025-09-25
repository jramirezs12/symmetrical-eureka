using MongoDB.Bson.Serialization.Attributes;

namespace RulesEngine.Domain.Agregations.Entities
{
    public class ResearchRequest
    {
        [BsonElement("RadNumbers")]
        public string[]? RadNumbers { get; set; }
        [BsonElement("Investigations")]
        public string[]? Investigations { get; set; }
        [BsonElement("HasInvestigation")]
        public bool HasInvestigation { get; set; } = false;
        [BsonElement("AccidentNumber")]
        public string AccidentNumber { get; set; } = string.Empty;
    }
}
