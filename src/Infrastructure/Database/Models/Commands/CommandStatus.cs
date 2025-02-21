using System.ComponentModel.DataAnnotations;

namespace YAGO.World.Infrastructure.Database.Models.Commands
{
    public enum CommandStatus
    {
        [Display(Name = "Уничтожен")]
        Destroyed = -1,

        [Display(Name = "Готов к движению")]
        ReadyToMove = 100,

        [Display(Name = "Отступление")]
        Retreat = 140,

        [Display(Name = "Завершено")]
        Complited = 150
    }
}
