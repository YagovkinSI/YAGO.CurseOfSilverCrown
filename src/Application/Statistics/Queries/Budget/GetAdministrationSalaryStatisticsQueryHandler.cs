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
    public class GetAdministrationSalaryStatisticsQueryHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetAdministrationSalaryStatisticsQuery, GetStatisticsResult>
    {
        public async Task<GetStatisticsResult> Handle(
            GetAdministrationSalaryStatisticsQuery query,
            CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(query.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");

            var fields = new List<StatisticFieldDto>
            {
                GetRulerSalary(colony),

                GetTotal(colony),
            };

            var statistics = new StatisticsResult(
                StatisticCode.SolarDelta,
                $"Расходы на администрацию",
                fields);
            return new GetStatisticsResult(statistics);
        }

        private static StatisticFieldDto GetRulerSalary(Colony colony)
        {
            var value = colony.State.Achievements.HasAchievement(AchievementConstants.RulerContractSigned)
                ? -GameConstants.RulerSalary
                : 0;
            return new(
                ParameterCategory.SolarDelta,
                "Зарплата правителя",
                $"{value.ToBeautifulString(setPlus: true)}",
                value.ToStatusByZero(),
                Info: new DisplayInfo(
                    "Зарплата правителя",
                    description: [
                        "Зарплата правителя станции оплачивается из бюджета колонии."]),
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetTotal(Colony colony)
        {
            var value = colony.GetAdministrationSalary();
            return new(
                ParameterCategory.SolarDelta,
                "ИТОГО",
                $"{(-value).ToBeautifulString(setPlus: true)}",
                value.ToStatusByZero(invert: true),
                Info: new DisplayInfo(
                    "Администрация",
                    description: [
                        "Расходы на содержание администрации колонии за год."]),
                ChildrenCode: null);
        }
    }

    public record GetAdministrationSalaryStatisticsQuery(long UserId) : IRequest<GetStatisticsResult>;
}
