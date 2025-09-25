using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace RulesEngine.Domain.Helper;

public class StringOrIntToLongConverter : SerializerBase<long?>
{
    public override long? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonType = context.Reader.GetCurrentBsonType();
        switch (bsonType)
        {
            case BsonType.String:
                var strValue = context.Reader.ReadString();
                if (string.IsNullOrWhiteSpace(strValue) || string.IsNullOrEmpty(strValue))
                {
                    return 0;
                }
                if (long.TryParse(strValue, out var longValue))
                {
                    return longValue;
                }
                return 0;
            case BsonType.Int32:
                return context.Reader.ReadInt32();
            case BsonType.Int64:
                return context.Reader.ReadInt64();
            case BsonType.Double:
                return (long)context.Reader.ReadDouble();
            case BsonType.Null:
                context.Reader.ReadNull();
                return 0;
            default:
                context.Reader.SkipValue();
                return 0;
        }
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, long? value)
    {
        if (value == null)
        {
            context.Writer.WriteInt64(0); // Si el valor es null, guarda 0 en la base de datos
        }
        else
        {
            context.Writer.WriteInt64(value.Value);
        }
    }
}