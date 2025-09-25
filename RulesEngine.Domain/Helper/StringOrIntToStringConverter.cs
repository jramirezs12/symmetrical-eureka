using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace RulesEngine.Domain.Mundial.Invoices.Helper;

public class StringOrIntToStringConverter : SerializerBase<string>
{
    public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonType = context.Reader.GetCurrentBsonType();
        switch (bsonType)
        {
            case BsonType.String:
                var strValue = context.Reader.ReadString();
                return string.IsNullOrWhiteSpace(strValue) ? null! : strValue;
            case BsonType.Int32:
                return context.Reader.ReadInt32().ToString();
            case BsonType.Int64:
                return context.Reader.ReadInt64().ToString();
            case BsonType.Double:
                return context.Reader.ReadDouble().ToString();
            case BsonType.Null:
                context.Reader.ReadNull();
                return null!;
            default:
                return string.Empty;
        }
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
    {
        if (string.IsNullOrEmpty(value) || (string.IsNullOrWhiteSpace(value)))
            context.Writer.WriteNull();
        else if (int.TryParse(value, out var intValue))
            context.Writer.WriteInt32(intValue);
        else
            context.Writer.WriteString(value);
    }
}