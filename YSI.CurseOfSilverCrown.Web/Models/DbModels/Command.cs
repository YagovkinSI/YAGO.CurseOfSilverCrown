using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Enums;

namespace YSI.CurseOfSilverCrown.Web.Models.DbModels
{
    public class Command
    {
        public string Id { get; set; }
        public string OrganizationId { get; set; }
        [Display(Name = "Казна")]
        public int Coffers { get; set; }
        [Display(Name = "Воины")]
        public int Warriors { get; set; }

        [Display(Name = "Действие")]
        public enCommandType Type { get; set; }
        [Display(Name = "Цель")]
        public string TargetOrganizationId { get; set; }

        public Organization Organization { get; set; }
        public Organization Target { get; set; }

        internal bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
