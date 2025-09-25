using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Domain.InputSourcesEntities
{
    public class IpsInvestigation
    {
        public required string IpsNit { get; set; }
        public required string IpsName { get; set; }
        public required string StartDate { get; set; }
        public required string EndDate { get; set; }
    }
}
