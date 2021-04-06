using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Web.Enums
{
    public enum enEventParametrChange
    {
        [Display(Name = "Воины")]
        Warrior = 1,

        [Display(Name = "Казна")]
        Coffers = 2,

    }
}
