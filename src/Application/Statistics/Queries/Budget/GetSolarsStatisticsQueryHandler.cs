using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Extensions;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Application.Statistics.Queries.Models;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Application.Statistics.Queries.Budget
{
    public class GetSolarsStatisticsQueryHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetSolarsStatisticsQuery, GetStatisticsResult>
    {
        public async Task<GetStatisticsResult> Handle(
            GetSolarsStatisticsQuery query,
            CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(query.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");

            var fields = new List<StatisticFieldDto>
            {
                GetCurrent(colony),
                GetBudget(colony),
                GetDelta(colony)
            };

            var statistics = new StatisticsResult(
                StatisticCode.SolarDelta,
                $"Солары",
                fields);
            return new GetStatisticsResult(statistics);
        }

        private static StatisticFieldDto GetCurrent(Colony colony)
        {
            var value = colony.State.Resources.Solars.Value;
            return new(
                ParameterCategory.Solars,
                "Текущий баланс",
                $"{value.ToBeautifulString()}",
                value.ToStatusByZero(),
                Info: new DisplayInfo(
                    "Солары  (SOL)",
                    description: [
                        "Расчётная валюта колонии. Используются для строительства, зарплат и заключения контрактов."]),
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetBudget(Colony colony)
        {
            var value = colony.GetSolarDeltaPerYear();
            return new(
                ParameterCategory.SolarDelta,
                "Бюджет",
                $"{value.ToBeautifulString(setPlus: true)} в год",
                value.ToStatusByZero(),
                Info: new DisplayInfo(
                    "Бюджет",
                    description: [
                        "Итоговое сальдо бюджета колонии за год."]),
                ChildrenCode: StatisticCode.SolarDelta);
        }

        private static StatisticFieldDto GetDelta(Colony colony)
        {
            var value = colony.GetSolarDelta();
            return new(
                ParameterCategory.SolarDelta,
                "Солары за ход",
                $"{value.ToBeautifulString(setPlus: true)}",
                value.ToStatusByZero(),
                Info: new DisplayInfo(
                    "Солары за ход",
                    description: [
                        "Изменение соларов колонии каждый ход. Часть годового бюджета."]),
                ChildrenCode: null);
        }
    }

    public record GetSolarsStatisticsQuery(long UserId) : IRequest<GetStatisticsResult>;
}
