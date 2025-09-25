using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Domain.InputSourcesEntities
{
    public class FraudulentIps
    {
        public required string IpsNit { get; set; }
        public required string IpsName { get; set; }
        public required string NotificationDate { get; set; }
        public required string Result { get; set; }
    }
}
