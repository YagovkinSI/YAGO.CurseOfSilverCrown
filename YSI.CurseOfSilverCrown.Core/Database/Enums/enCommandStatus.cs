using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Core.Database.Enums
{
    public enum enCommandStatus
    {
        ForDelete = -1,

        [Display(Name = "Готов к отправке")]
        ReadyToSend = 0,

        //[Display(Name = "На рассмотрении")]
        //UnderСonsideration = 10,

        //[Display(Name = "Отказ")]
        //Renouncement = 20,

        [Display(Name = "Готов к выполнению")]
        ReadyToRun = 100,

        //[Display(Name = "Перемещение к цели")]
        //MovingToGoal = 110,

        //[Display(Name = "Выполнение основной фазы")]
        //MainPhaseExecution = 120,

        //[Display(Name = "Выполнение дополнительной фазы")]
        //AdditionalPhaseExecution = 130,

        //[Display(Name = "Возвращение")]
        //Returning = 140,

        //[Display(Name = "Завершено")]
        //Complited = 150
    }
}
