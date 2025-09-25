using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RulesEngine.Persistence.Settings;

namespace RulesEngine.Persistence.Configurations.DataBaseConfiguration
{
    public class GiproDbContext(IOptions<GiproConnectionSettings> options)
    {
        public readonly IMongoClient _client = new MongoClient(options.Value.ConnectionString);

        public IMongoClient DbContext => _client;
    }
}