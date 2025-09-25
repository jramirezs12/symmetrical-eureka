using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Domain.Agregations.Entities
{
    public class SinisterAggregation
    {
        public string _id { get; set; } = string.Empty;
        public int Count { get; set; }
        public string[]? RadNumbers { get; set; }
    }
}
