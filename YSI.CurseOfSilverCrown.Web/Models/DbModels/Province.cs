using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Enums;

namespace YSI.CurseOfSilverCrown.Web.Models.DbModels
{
    public class Province
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Organization> Organizations { get; set; }
    }
}
