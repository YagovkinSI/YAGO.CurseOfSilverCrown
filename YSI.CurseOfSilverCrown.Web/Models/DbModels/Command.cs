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
        public int TurnId { get; set; }
        public string OrganizationId { get; set; }

        [Display(Name = "Действие")]
        public enCommandType Type { get; set; }
        [Display(Name = "Цель")]
        public string TargetOrganizationId { get; set; }
        public string Result { get; set; }

        public Turn Turn { get; set; }
        public Organization Organization { get; set; }
        public Organization Target { get; set; }
    }
}
