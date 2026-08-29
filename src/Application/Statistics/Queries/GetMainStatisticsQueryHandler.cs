using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Extensions;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Application.Statistics.Queries.Models;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Slots;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Application.Statistics.Queries
{
    public class GetMainStatisticsQueryHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetMainStatisticsQuery, GetStatisticsResult>
    {
        public async Task<GetStatisticsResult> Handle(
            GetMainStatisticsQuery query,
            CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(query.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");

            var fields = new List<StatisticFieldDto>
            {
                GetFieldActionPoints(colony),
                GetFieldSolars(colony),
                GetFieldModules(colony),

                GetFieldSolarDelta(colony),
                GetFieldMood(colony),

                GetFieldMainMore(),
            };

            var statistics = new StatisticsResult(
                StatisticCode.Main,
                $"Колония: {colony.DisplayInfo.DisplayName}",
                fields);
            return new GetStatisticsResult(statistics);
        }

        private static StatisticFieldDto GetFieldActionPoints(Colony colony)
        {
            return new(
                ParameterCategory.ActionPoints,
                "Очки Действий",
                $"{colony.State.Resources.ActionPoints.Value.ToBeautifulString()}/" +
                    $"{colony.State.Resources.ActionPoints.MaxValue.ToBeautifulString()} " +
                    $"(+{colony.State.Resources.ActionPoints.GetDeltaPerTurn(colony.State).ToBeautifulString()} за ход)",
                ParameterStatus.Neutral,
                Info: null,
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldSolars(Colony colony)
        {
            return new(
                ParameterCategory.Solars,
                "Солары",
                $"{colony.State.Resources.Solars.Value.ToBeautifulString()}",
                ParameterStatus.Neutral,
                Info: new DisplayInfo(
                    "Солары",
                    imageName: ImageSet.TrendOnDisplay,
                    description: [
                        "Солар (SOL) — внутренняя расчётная единица Консорциума Пояса. Введена в 2062 году по инициативе Дориана Восса, когда Консорциум преобразовался в единое акционерное общество. Это частная цифровая валюта, которая не является официальным платёжным средством ни одного из государств Земли.",
                        "Примерный курс на 2073 год: 1 SOL ≈ $13 400. Высокая стоимость объясняется тем, что в Поясе деньги тратятся на оборудование, перелёты и контракты с корпорациями — суммы там исчисляются сотнями и тысячами SOL. Основное преимущество Соларов — стабильность: в отличие от земных валют, он практически не подвержен инфляции.",
                        "Солар принимается на большинстве станций Пояса, включая Цереру, Психею и Весту. Им пользуются независимые колонии и даже Чёрная Марка. А на Земле по-прежнему платят долларами, юанями и евро."]),
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldModules(Colony colony)
        {
            return new(
                ParameterCategory.Modules,
                "Модули",
                $"{colony.State.Slots[ColonySlotType.Modules].GetUsed(colony.State).ToBeautifulString()}/" +
                    $"{colony.State.Slots[ColonySlotType.Modules].GetTotal(colony.State).ToBeautifulString()}",
                ParameterStatus.Neutral,
                Info: null,
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldSolarDelta(Colony colony)
        {
            var solarDelta = colony.GetSolarDelta();
            return new(
                ParameterCategory.SolarDelta,
                "Бюджет",
                $"{solarDelta.ToBeautifulString(setPlus: true)} солар/ход",
                solarDelta > 0 ? ParameterStatus.Neutral : ParameterStatus.Bad,
                Info: null,
                ChildrenCode: StatisticCode.SolarDelta);
        }

        private static StatisticFieldDto GetFieldMood(Colony colony)
        {
            var value = colony.State.Resources.Mood.Value;
            return new(
                ParameterCategory.Mood,
                "Доверие",
                $"{value.ToBeautifulString()} " +
                    $"({colony.State.Resources.Mood.GetDeltaPerTurn(colony.State).ToBeautifulString()} за ход)",
                value > GameEventConstants.TrustWithRevolt ? ParameterStatus.Neutral : ParameterStatus.Bad,
                Info: null,
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldMainMore()
        {
            return new(
                ParameterCategory.Info,
                "Дополнительно",
                Value: "",
                ParameterStatus.Neutral,
                Info: null,
                ChildrenCode: StatisticCode.MainMore);
        }
    }

    public record GetMainStatisticsQuery(long UserId) : IRequest<GetStatisticsResult>;
}
