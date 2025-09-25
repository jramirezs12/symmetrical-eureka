using MongoDB.Bson.Serialization.Attributes;
using RulesEngine.Domain.Primitives;

namespace RulesEngine.Domain.Provider.Entities
{
    public class ProviderData : Entity<string>
    {
        [BsonElement("NitIps")]
        public string NitIps { get; set; } = string.Empty;
        [BsonElement("NameIps")]
        public string NameIps { get; set; } = string.Empty;
        [BsonElement("HabilitationCode")]
        public string HabilitationCode { get; set; } = string.Empty;
    }
}
