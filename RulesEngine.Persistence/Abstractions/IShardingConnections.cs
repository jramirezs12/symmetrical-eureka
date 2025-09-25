using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Persistence.Abstractions
{
    public interface IShardingConnections
    {
        Dictionary<string, string?> GetNamedConnectionString(string connectionName);
        Dictionary<string, string?> GetNamedConnections();
    }
}
