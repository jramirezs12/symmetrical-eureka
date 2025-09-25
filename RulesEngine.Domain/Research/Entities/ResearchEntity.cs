using MongoDB.Bson.Serialization.Attributes;
using RulesEngine.Domain.Invoices.Entities;
using RulesEngine.Domain.Primitives;

namespace RulesEngine.Domain.Research.Entities
{
    [BsonIgnoreExtraElements]
    public class ResearchEntity : Entity<string>
    {
        [BsonElement("Request")]
        public Request? Request { get; set; }
        [BsonElement("Response")]
        public ResponseResearchs? Response { get; set; }
        [BsonElement("OriginModule")]
        public string? OriginModule { get; set; } = string.Empty;
        [BsonElement("CreationDate")]
        public DateTime? CreationDate { get; set; }
        [BsonElement("ResponseDate")]
        public DateTime? ResponseDate { get; set; }
        [BsonElement("UserRequest")]
        public object? UserRequest { get; set; }
        [BsonElement("UserResponse")]
        public string? UserResponse { get; set; } = string.Empty;
        public DateTime? ExpirationDate { get; set; }
    }
}
