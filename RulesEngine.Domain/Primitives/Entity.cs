using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Domain.Primitives
{
    public abstract class Entity<TEntityId>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public TEntityId? Id { get; init; }

        protected Entity(TEntityId id)
        {
            Id = id;
        }

        protected Entity() { }
    }
}
