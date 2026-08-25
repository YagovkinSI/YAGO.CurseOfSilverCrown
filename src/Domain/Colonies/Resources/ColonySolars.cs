using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.Colonies.Resources
{
    public class ColonySolars : ColonyResource<double>, IDisplayInfo
    {
        public override double MinValue => double.MinValue;
        public override double MaxValue => double.MaxValue;
        public DisplayInfo DisplayInfo { get; }

        public ColonySolars(double value) : base(value)
        {
            DisplayInfo = CreateDisplayInfo();
        }

        private DisplayInfo CreateDisplayInfo()
        {
            return new DisplayInfo(
                name: "Солар (SOL)",
                ImageSet.Show_StendUp,
                description:
                [
                    "Солар (SOL) — внутренняя расчётная единица Консорциума Пояса. Введена в 2062 году по инициативе Дориана Восса, когда Консорциум преобразовался в единое акционерное общество. Это частная цифровая валюта, которая не является официальным платёжным средством ни одного из государств Земли.",
                    "Примерный курс на 2073 год: 1 SOL ≈ $13 400. Высокая стоимость объясняется тем, что в Поясе деньги тратятся на оборудование, перелёты и контракты с корпорациями — суммы там исчисляются сотнями и тысячами SOL. Основное преимущество Соларов — стабильность: в отличие от земных валют, он практически не подвержен инфляции.",
                    "Солар принимается на большинстве станций Пояса, включая Цереру, Психею и Весту. Им пользуются независимые колонии и даже Чёрная Марка. А на Земле по-прежнему платят долларами, юанями и евро."
                ]);
        }
    }
}
