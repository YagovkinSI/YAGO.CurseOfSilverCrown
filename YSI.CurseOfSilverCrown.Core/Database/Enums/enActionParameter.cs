using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Core.Database.Enums
{
    public enum enActionParameter
    {
        [Display(Name = "Воины (всего)")]
        Warrior = 1,

        [Display(Name = "Казна")]
        Coffers = 2,

        [Display(Name = "Имущество владения")]
        Investments = 3,

        [Display(Name = "Укрепления")]
        Fortifications = 4,

        [Display(Name = "Воины учавствовавшие в боях")]
        WarriorInWar = 1001,

    }
}
