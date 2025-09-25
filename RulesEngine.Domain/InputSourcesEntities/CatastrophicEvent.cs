using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Domain.InputSourcesEntities
{
    public class CatastrophicEvent

    {
        public required string SoatNumber { get; set; }
        public required string UploadDate { get; set; }
        public required string Alert { get; set; }
    }
}
