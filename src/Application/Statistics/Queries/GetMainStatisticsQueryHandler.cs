using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Extensions;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Application.Statistics.Queries.Models;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Slots;
using YAGO.World.Domain.Common;
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

                GetFieldMood(colony),

                GetFieldMainMore(),
            };

            var statistics = new StatisticsResult(
                StatisticCode.Main,
                $"Основная информация",
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
                    $"(+{colony.State.Resources.ActionPoints.GetDeltaPerTurn(colony.State).ToBeautifulString()})",
                ParameterStatus.Neutral,
                Info: new DisplayInfo(
                    "Очки Действий (ОД)",
                    description: [
                        "Лимит действий на текущий ход. Строительство, приказы и реформы тратят ОД, а в начале нового хода они восстанавливаются."]),
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldSolars(Colony colony)
        {
            var value = colony.State.Resources.Solars.Value;
            var delta = colony.GetSolarDelta();
            var afterTenTurns = value + delta * 10;
            var status = afterTenTurns.ToStatusByZero();
            return new(
                ParameterCategory.Solars,
                "Солары",
                $"{value.ToBeautifulString()} ({delta.ToBeautifulString(setPlus: true)})",
                status,
                Info: new DisplayInfo(
                    "Солары (SOL)",
                    description: [
                        "Расчётная валюта колонии. Используются для строительства, зарплат и заключения контрактов."]),
                ChildrenCode: StatisticCode.Solars);
        }

        private static StatisticFieldDto GetFieldModules(Colony colony)
        {
            return new(
                ParameterCategory.Modules,
                "Модули",
                $"{colony.State.Slots[ColonySlotType.Modules].GetUsed(colony.State).ToBeautifulString()}/" +
                    $"{colony.State.Slots[ColonySlotType.Modules].GetTotal(colony.State).ToBeautifulString()}",
                ParameterStatus.Neutral,
                Info: new DisplayInfo(
                    "Модули",
                    description: [
                        "Слоты станции для размещения построек. Показаны занятые слоты и лимит станции."]),
                ChildrenCode: null);
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
                Info: new DisplayInfo(
                    "Доверие",
                    description: [
                        "Уровень поддержки населения. При низких значениях растёт риск протестов и бунтов."]),
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
