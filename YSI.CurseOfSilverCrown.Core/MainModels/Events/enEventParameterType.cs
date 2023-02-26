using System.ComponentModel.DataAnnotations;

namespace YSI.CurseOfSilverCrown.Core.MainModels.Events
{
    public enum enEventParameterType
    {
        [Display(Name = "Воины (всего)")]
        Warrior = 1,

        [Display(Name = "Казна")]
        Coffers = 2,

        [Display(Name = "Имущество владения")]
        Investments = 3,

        [Display(Name = "Укрепления")]
        Fortifications = 4,

        [Display(Name = "Воины участвовавшие в боях")]
        WarriorInWar = 1001,

        [Display(Name = "Воины бывшие во владении")]
        WarriorInDomain = 1002,

    }
}
