using System.ComponentModel.DataAnnotations;

namespace YSI.CurseOfSilverCrown.Web.Database.Commands
{
    public enum CommandType
    {
        ForDelete = -1,

        [Display(Name = "Расходы двора")]
        Idleness = 11,

        [Display(Name = "Сбор войск")]
        Growth = 1,

        [Display(Name = "Вложение средств в имущество владения")]
        Investments = 4,

        [Display(Name = "Передача вассала")]
        VassalTransfer = 6,

        [Display(Name = "Укрепления")]
        Fortifications = 7,

        [Display(Name = "Отправка золота")]
        GoldTransfer = 8,

        [Display(Name = "Восстание против сюзерена")]
        Rebellion = 9,
    }
}
