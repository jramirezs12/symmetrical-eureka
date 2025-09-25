using MongoDB.Bson.Serialization.Attributes;
using RulesEngine.Domain.Primitives;

namespace RulesEngine.Domain.Constants.Entities
{
    public class ConstantsEntity : Entity<string>
    {
        public string? BusinessCode { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime UpdateTime { get; set; }
        public bool State { get; set; }
        public List<ListType>? ListType { get; set; }

    }

    public class ListType
    {
        [BsonElement("_id")]
        [BsonId]
        public int _id { get; set; }
        public string? Code{ get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public bool State { get; set; }
    }
}
