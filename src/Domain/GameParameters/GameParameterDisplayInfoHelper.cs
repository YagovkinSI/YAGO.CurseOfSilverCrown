using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.GameParameters
{
    public static class GameParameterDisplayInfoHelper
    {
        public static DisplayInfo GetDisplayInfo(this GameParameterType parameterType)
        {
            return parameterType switch
            {
                GameParameterType.SolarsCurrent => new DisplayInfo(
                    name: "Солар (SOL)",
                    ImageSet.Show_StendUp,
                    description:
                    [
                        "Солар (SOL) — внутренняя расчётная единица Консорциума Пояса. Введена в 2062 году по инициативе Дориана Восса, когда Консорциум преобразовался в единое акционерное общество. Это частная цифровая валюта, которая не является официальным платёжным средством ни одного из государств Земли.",
                        "Примерный курс на 2073 год: 1 SOL ≈ $13 400. Высокая стоимость объясняется тем, что в Поясе деньги тратятся на оборудование, перелёты и контракты с корпорациями — суммы там исчисляются сотнями и тысячами SOL. Основное преимущество Соларов — стабильность: в отличие от земных валют, он практически не подвержен инфляции.",
                        "Солар принимается на большинстве станций Пояса, включая Цереру, Психею и Весту. Им пользуются независимые колонии и даже Чёрная Марка. А на Земле по-прежнему платят долларами, юанями и евро."
                    ]),
                GameParameterType.SolarsDelta => new DisplayInfo("Доход колонии"),
                GameParameterType.SolarDeltaIndustriesPrivate => new DisplayInfo("Частные компании"),
                GameParameterType.SolarDeltaIndustriesState => new DisplayInfo("Бюджетные компании"),
                GameParameterType.PublicDebtService => new DisplayInfo("Платеж по долгу"),
                GameParameterType.AdministrationSalary => new DisplayInfo("Госаппарат"),
                GameParameterType.PopulationTaxSolars => new DisplayInfo("Налоги с населения"),
                GameParameterType.ActionPointsCurrent => new DisplayInfo("Очки действий"),
                GameParameterType.ActionPointsDelta => new DisplayInfo("Очки действий за ход"),
                GameParameterType.ModulesTotal => new DisplayInfo("Модулей всего"),
                GameParameterType.ModulesUsed => new DisplayInfo("Модулей занято"),
                GameParameterType.MoodCurrent => new DisplayInfo("Доверие"),
                GameParameterType.MoodDelta => new DisplayInfo("Доверие за ход"),
                GameParameterType.MiningSlotsFree => new DisplayInfo("Свободных зон добычи"),
                GameParameterType.TurnsCurrent => new DisplayInfo("Ход"),
                GameParameterType.Population => new DisplayInfo("Население"),
                GameParameterType.ReformsTaxLevel => new DisplayInfo("Уровень налога"),
                GameParameterType.ReformsSocialGuaranteesLevel => new DisplayInfo("Уровень соц.гарантий"),
            };
        }
    }
}
