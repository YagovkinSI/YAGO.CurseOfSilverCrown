using System.ComponentModel.DataAnnotations;

namespace YAGO.World.Domain.Units.Enums
{
    public enum UnitCommandType
    {
        ForDelete = -1,

        [Display(Name = "Нападение")]
        War = 2,

        [Display(Name = "Роспуск")]
        Disbandment = 3,

        [Display(Name = "Защита владения")]
        WarSupportDefense = 5,

        [Display(Name = "Помощь в нападении")]
        WarSupportAttack = 10
    }
}
