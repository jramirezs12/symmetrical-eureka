using MongoDB.Bson.Serialization.Attributes;
using RulesEngine.Domain.Primitives;

namespace RulesEngine.Domain.ElectronicBillingRuleEngine.Entities
{
    public class ElectronicBillingEntity : Entity<string>
    {
        [BsonElement("NitIps")]
        public string NitIps { get; set; } = string.Empty;
        [BsonElement("NameIps")]
        public string NameIps { get; set; } = string.Empty;
        [BsonElement("InvoiceNumber")]
        public string InvoiceNumber { get; set; } = string.Empty;
        [BsonElement("TotalValue")]
        public int TotalValue { get; set; } = 0;
        [BsonElement("EmisionDate")]
        public DateTime? EmisionDate { get; set; }
        [BsonElement("InvoiceNumberHR")]
        public string InvoiceNumberHR { get; set; } = string.Empty;
        [BsonElement("Observation")]
        public string Observation { get; set; } = string.Empty;
        [BsonElement("SupplyDeliveyDate")]
        public DateTime? SupplyDeliveyDate { get; set; }
    }
}
