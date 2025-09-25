using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RulesEngine.Persistence.Settings;

namespace RulesEngine.Persistence.Configurations.DataBaseConfiguration
{
    public class BasicGiproDbContext
    {
        public readonly IMongoClient _client;

        public BasicGiproDbContext(IOptions<BasicMongoConnectionSettings> options)
        {
            _client = new MongoClient(options.Value.ConnectionString);
        }

        public IMongoClient _dbContext => _client;
    }
}
