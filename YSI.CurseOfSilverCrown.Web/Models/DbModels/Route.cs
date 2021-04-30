using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Web.Models.DbModels
{
    public class Route
    {
        public int FromProvinceId { get; set; }
        public int ToProvinceId { get; set; }

        public Province FromProvince { get; set; }
        public Province ToProvince { get; set; }
    }
}
