using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Core.Database.Models
{
    public class DomainRelation
    {
        public int Id { get; set; }
        public int SourceDomainId { get; set; }

        [Display(Name = "Владение")]
        public int TargetDomainId { get; set; }

        [Display(Name = "Включая его вассалов?")]
        public bool IsIncludeVassals { get; set; }

        [Display(Name = "Разшение на право прохода по своей территории")]
        public bool PermissionOfPassage { get; set; }

        public Domain SourceDomain { get; set; }
        public Domain TargetDomain { get; set; }
    }
}
