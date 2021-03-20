using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Web.Enums
{
    public enum enCommandType
    {
        [Display(Name = "Бездействие")]
        Idleness = 0,

        [Display(Name = "Развитие")]
        Growth = 1,
        [Display(Name = "Нападение")]
        War = 2,

        Defense = 2000,

        VassalTax = 10000,
        SuzerainIncome = 10001,
    }

    public enum enCommandTypeForChoose
    {
        [Display(Name = "Бездействие")]
        Idleness = 0,

        [Display(Name = "Развитие")]
        Growth = 1,
        [Display(Name = "Нападение")]
        War = 2
    }
}
