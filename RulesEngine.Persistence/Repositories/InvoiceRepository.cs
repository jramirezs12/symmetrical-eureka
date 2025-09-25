using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using RulesEngine.Domain.Invoices.Entities;
using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Persistence.Abstractions;
using RulesEngine.Persistence.Configurations.DataBaseConfiguration;

namespace RulesEngine.Persistence.Repositories
{
    public class InvoiceRepository : MongoRepository<Invoice, string>, IInvoiceRepository
    {
        public InvoiceRepository(GiproDbContext context,
            ICurrentTenantService tenantService) : base(context, tenantService.TenantId, "Invoice")
        {}

        public async Task<Dictionary<string, object>> GetInvoiceByAggregation(BsonDocument[] aggregate,
                                                                              List<BsonElement> elements)
        {
            var cursor = _collection.Aggregate<BsonDocument>(aggregate);
            var result = await cursor.FirstOrDefaultAsync();

            if (result != default)
            {
                foreach (var element in elements)
                {
                    if (result.Contains(element.Name))
                    {
                        var bsonObject = result[element.Name];
                        if (bsonObject.IsBsonArray)
                        {
                            foreach (var value in bsonObject.AsBsonArray)
                                result[element.Name] = value;

                            if (result[element.Name].IsBsonArray)
                                result[element.Name] = BsonValue.Create(null);
                        }
                    }
                }
            }
            return result is not null ? result.ToDictionary() : new Dictionary<string, object>();
        }

        public async Task<BsonDocument> GetInvoiceByAggregation(BsonDocument[] bsonElements)
        {
            IAsyncCursor<BsonDocument> cursor = await _collection.AggregateAsync< BsonDocument>(bsonElements, new AggregateOptions { AllowDiskUse = true });

            BsonDocument invoice = await cursor.FirstOrDefaultAsync();
            return invoice;
        }
    }
}
