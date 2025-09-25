using MongoDB.Bson.Serialization.Attributes;

namespace RulesEngine.Domain.Invoices.Entities;

[BsonNoId]
public class CommonType
{
    public int Id { get; set; } = 0;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool State { get; set; }
}
