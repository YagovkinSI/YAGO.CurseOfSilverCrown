using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Core.Database.Enums
{
    public enum enCommandStatus
    {
        [Display(Name = "Готов к движению")]
        ReadyToMove = 100,

        //[Display(Name = "На позиции и котов к выполнению")]
        //InPositionToDo = 110,

        //[Display(Name = "Готов к движению")]
        //ReadyToMove = 100,

        //[Display(Name = "Готов к движению")]
        //ReadyToMove = 100,

        [Display(Name = "Завершено")]
        Complited = 150
    }
}
