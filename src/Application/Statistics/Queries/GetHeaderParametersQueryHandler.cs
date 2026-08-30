using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Extensions;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Application.Statistics.Queries.Models;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Slots;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Application.Statistics.Queries
{
    public class GetHeaderParametersQueryHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetHeaderParametersQuery, List<StatisticFieldDto>>
    {
        public async Task<List<StatisticFieldDto>> Handle(
            GetHeaderParametersQuery query,
            CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(query.UserId, cancellationToken);
            if (colony == null || !colony.State.Achievements.HasAchievement(AchievementConstants.RulerContractSigned))
                return [];

            return
            [
                GetFieldActionPoints(colony),
                GetFieldSolars(colony),
                GetFieldModules(colony),
                GetFieldSolarDelta(colony),
                GetFieldMood(colony),
            ];
        }

        private static StatisticFieldDto GetFieldActionPoints(Colony colony)
        {
            var value = colony.State.Resources.ActionPoints.Value;
            return new(
                ParameterCategory.ActionPoints,
                "Очки Действий",
                $"{value.ToBeautifulString()}/" +
                    $"{colony.State.Resources.ActionPoints.MaxValue.ToBeautifulString()}",
                value > 0 ? ParameterStatus.Good : ParameterStatus.Neutral,
                Info: null,
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldSolars(Colony colony)
        {
            var value = colony.State.Resources.Solars.Value;
            var status = value switch
            {
                >= 1000 => ParameterStatus.Good,
                <= 0 => ParameterStatus.Bad,
                _ => ParameterStatus.Neutral,
            };
            return new(
                ParameterCategory.Solars,
                "Солары",
                $"{value.ToBeautifulString()}",
                status,
                Info: null,
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldModules(Colony colony)
        {
            var freeModules = colony.State.Slots[ColonySlotType.Modules].GetFree(colony.State);
            return new(
                ParameterCategory.Modules,
                "Модули",
                $"{colony.State.Slots[ColonySlotType.Modules].GetUsed(colony.State).ToBeautifulString()}/" +
                    $"{colony.State.Slots[ColonySlotType.Modules].GetTotal(colony.State).ToBeautifulString()}",
                freeModules > 4 ? ParameterStatus.Good : ParameterStatus.Neutral,
                Info: null,
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldSolarDelta(Colony colony)
        {
            var value = colony.GetSolarDelta();
            var status = value switch
            {
                > 0 => ParameterStatus.Good,
                < 0 => ParameterStatus.Bad,
                _ => ParameterStatus.Neutral,
            };
            return new(
                ParameterCategory.Solars,
                "Бюджет",
                $"{value.ToBeautifulString(setPlus: true)}",
                status,
                Info: null,
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldMood(Colony colony)
        {
            var value = colony.State.Resources.Mood.Value;
            var status = value switch
            {
                <= GameEventConstants.TrustWithRevoltCritical => ParameterStatus.Critical,
                <= GameEventConstants.TrustWithRevolt => ParameterStatus.Bad,
                _ => ParameterStatus.Neutral
            };
            return new(
                ParameterCategory.Mood,
                "Доверие",
                $"{value.ToBeautifulString()}",
                status,
                Info: null,
                ChildrenCode: null);
        }
    }

    public record GetHeaderParametersQuery(long UserId) : IRequest<List<StatisticFieldDto>>;
}
