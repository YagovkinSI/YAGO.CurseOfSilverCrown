using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Core.Database.Models
{
    public class Route
    {
        public int FromDomainId { get; set; }
        public int ToDomainId { get; set; }

        public Domain FromDomain { get; set; }
        public Domain ToDomain { get; set; }
    }
}
