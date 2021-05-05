using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.Core.Database.Models
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

        [Display(Name = "Дополнительная цель")]
        public string Target2OrganizationId { get; set; }

        public Organization Organization { get; set; }
        public Organization Target { get; set; }
        public Organization Target2 { get; set; }

        internal bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
