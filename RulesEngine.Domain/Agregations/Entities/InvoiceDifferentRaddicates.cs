using MongoDB.Bson.Serialization.Attributes;

namespace RulesEngine.Domain.Agregations.Entities
{
    public class InvoiceDifferentRadicates
    {
        [BsonElement("TotalRadNumbers")]
        public int TotalRadNumbers { get; set; } = 0;
        [BsonElement("RadNumbers")]
        public string[]? RadNumbers { get; set; }
    }
}
