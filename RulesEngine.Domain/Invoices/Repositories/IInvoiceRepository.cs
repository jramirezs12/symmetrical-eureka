using MongoDB.Bson;
using RulesEngine.Domain.Invoices.Entities;

namespace RulesEngine.Domain.Invoices.Repositories
{
    public interface IInvoiceRepository : IMongoRepository<Invoice, string>
    {
        Task<Dictionary<string, object>> GetInvoiceByAggregation(BsonDocument[] aggregate, List<BsonElement> elements);

        Task<BsonDocument> GetInvoiceByAggregation(BsonDocument[] bsonElements);
    }
}
