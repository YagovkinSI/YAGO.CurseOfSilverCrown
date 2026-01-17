using System;
using System.Linq;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Domain.Units
{
    public static class UnitsDataset
    {
        public static Unit[] Get()
        {
            return
            [
                GetMiningEngineeringTeam(),
                GetMiningBrigade(),
                GetMiningRehabilitationContingent(),
            ];
        }

        private static Unit GetMiningEngineeringTeam()
        {
            return new Unit(
                id: 1,
                name: "Инженерная Команда",
                cost: 2000,
                zonesOccupied: 10,
                solarsIncome: 40,
                gavernorType: ColonyPresetType.Humanist,
                population: 80,
                text: ["Передовое оборудование AS и горстка высокооплачиваемых специалистов. Дорого, престижно, эффективно."],
                description: [
                        "Ваша стратегия — качество, а не количество. Вы закупаете новейшие буровые дроны у AUTOMATIC SYSTEMS и нанимаете немногочисленных, но блестящих инженеров-операторов, предлагая им контракты с условиями для переезда семей. Этот путь требует крупных начальных вложений и высоких текущих затрат, но закладывает основу для «Привилегированного» статуса YAGO и максимальной эффективности добычи в будущем. Вы строите не просто рудник, а демонстрацию технологического превосходства."
                    ]);
        }

        private static Unit GetMiningBrigade()
        {
            return new Unit(
                id: 2,
                name: "Горнодобывающая Бригада",
                cost: 2000,
                zonesOccupied: 10,
                solarsIncome: 45,
                gavernorType: ColonyPresetType.Pragmatist,
                population: 100,
                text: ["Надёжное оборудование, бригада лицензированных рудокопов ОПЗ. Сбалансированный и предсказуемый старт."],
                description: [
                        "Вы следуете устоявшемуся плану: закупаете проверенные виброкирки и бульдозер, а также нанимаете целую бригаду рудокопов через агентства с лицензией ОПЗ. Это не прорыв, а уверенный шаг. Такой подход сигнализирует регуляторам о вашей благонадёжности, что является самым быстрым путём к стабильному «Стандартному» рейтингу. Вы выбираете предсказуемость и раннюю окупаемость."
                    ]);
        }

        private static Unit GetMiningRehabilitationContingent()
        {
            return new Unit(
                id: 3,
                name: "Реабилитационный Контингент",
                cost: 2000,
                zonesOccupied: 10,
                solarsIncome: 50,
                gavernorType: ColonyPresetType.Tyrant,
                population: 120,
                text: ["Дешёвое оборудование, контингент должников ОПЗ и обязательный надзор. Дёшево, рискованно, требует жёсткого контроля."],
                description: [
                        "Ваш расчёт строится на предельной экономии. Вы приобретаете самое простое оборудование, а в качестве рабочей силы используете контингент по программе трудовой реабилитации ОПЗ — должников и заключённых. Понимая риски, вы одновременно нанимаете отряд надзирателей для поддержания порядка. Этот путь позволяет начать с минимальным капиталом, но ваш рейтинг YAGO, вероятно, надолго останется «под наблюдением», а управление колонией будет сведено к подавлению недовольства и контролю за дисциплиной."
                    ]);
        }

        public static Unit[] GetUnits(long[] unitIds)
        {
            var allUnits = Get();

            var entities = allUnits
                .Where(x => unitIds.Contains(x.Id))
                .ToList();

            return unitIds
                .Select(x => entities.Single(y => y.Id == x))
                .ToArray();
        }
    }
}
