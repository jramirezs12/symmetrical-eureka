using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using RulesEngine.Domain.Invoices.Entities;

namespace RulesEngine.Domain.Mundial.Invoices.Helper;
public class DeskriptorSerializer : SerializerBase<Deskriptor?>
{
    public override Deskriptor? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonType = context.Reader.GetCurrentBsonType();

        if (bsonType == BsonType.String)
        {
            var value = context.Reader.ReadString();
            return new Deskriptor { Value = value };
        }
        else if (bsonType == BsonType.Document)
            return BsonSerializer.Deserialize<Deskriptor>(context.Reader);

        context.Reader.SkipValue();
        return null;
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Deskriptor? value)
    {
        if (value != null)
        {
            BsonSerializer.Serialize(context.Writer, value);
        }
        else
        {
            context.Writer.WriteNull();
        }
    }
}
