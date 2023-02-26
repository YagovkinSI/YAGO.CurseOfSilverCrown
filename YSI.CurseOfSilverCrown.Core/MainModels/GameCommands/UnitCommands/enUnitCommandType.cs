using System.ComponentModel.DataAnnotations;

namespace YSI.CurseOfSilverCrown.Core.MainModels.GameCommands.UnitCommands
{
    public enum enUnitCommandType
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
