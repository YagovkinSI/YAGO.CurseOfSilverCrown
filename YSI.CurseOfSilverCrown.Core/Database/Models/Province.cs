using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Core.Database.Models
{
    public class Province
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        public List<Organization> Organizations { get; set; }
        public List<Route> RouteFromHere { get; set; }
        public List<Route> RouteToHere { get; set; }
    }
}
