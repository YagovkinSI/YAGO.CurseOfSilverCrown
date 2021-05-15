using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Core.Database.Enums
{
    public enum enCommandType
    {
        ForDelete = -1,

        [Display(Name = "Расходы двора")]
        Idleness = 11,

        [Display(Name = "Сбор войск")]
        Growth = 1,

        [Display(Name = "Вложение средства в экономику провинции")]
        Investments = 4,

        [Display(Name = "Передача вассала")]
        VassalTransfer = 6,

        [Display(Name = "Укрепления")] 
        Fortifications = 7,

        [Display(Name = "Отправка золота")]
        GoldTransfer = 8
    }
}
