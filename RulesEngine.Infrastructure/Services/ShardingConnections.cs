using Microsoft.Extensions.Options;
using RulesEngine.Persistence.Abstractions;
using RulesEngine.Persistence.Settings;

namespace RulesEngine.Infrastructure.Services
{
    public class ShardingConnections(IOptionsSnapshot<GiproConnectionSettings> connection,
                                     IOptionsSnapshot<BasicMongoConnectionSettings> basic) : IShardingConnections
    {
        private readonly GiproConnectionSettings _Connection = connection.Value;
        private readonly BasicMongoConnectionSettings _basic = basic.Value;

        public Dictionary<string, string?> GetNamedConnections()
        {
            var connections = new Dictionary<string, string?>();

            foreach (KeyValuePair<string, string> item in _Connection.DatabaseNameCollection!)
            {
                connections.Add(item.Key, item.Value);
            }
            return connections;
        }

        public Dictionary<string, string?> GetNamedConnectionString(string connectionName)
        {
            var connections = new Dictionary<string, string?>();

            string? GiproDbName = _Connection.DatabaseNameCollection.ContainsKey(connectionName)
            ? _Connection.DatabaseNameCollection[connectionName]
            : null;

            string? BasicDbName = _basic.DatabaseNameCollection.ContainsKey(connectionName)
            ? _basic.DatabaseNameCollection[connectionName]
            : null;

            connections.Add("GiproDbName", GiproDbName);
            connections.Add("BasicDbName", BasicDbName);
            return connections;
        }
    }
}