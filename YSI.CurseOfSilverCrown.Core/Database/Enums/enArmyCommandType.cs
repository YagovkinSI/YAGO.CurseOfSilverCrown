using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Core.Database.Enums
{
    public enum enArmyCommandType
    {
        ForDelete = -1,

        [Display(Name = "Нападение")]
        War = 2,

        [Display(Name = "Сбор налогов")]
        CollectTax = 3,

        [Display(Name = "Защита владения")]
        WarSupportDefense = 5,

        [Display(Name = "Помощь в нападении")]
        WarSupportAttack = 10
    }
}
