using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Extensions;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Application.Statistics.Queries.Models;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Application.Statistics.Queries
{
    public class GetSolarDeltaStatisticsQueryHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetSolarDeltaStatisticsQuery, GetStatisticsResult>
    {
        public async Task<GetStatisticsResult> Handle(
            GetSolarDeltaStatisticsQuery query,
            CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(query.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");

            var fields = new List<StatisticFieldDto>
            {
                GetIndustriesState(colony),
                GetIndustriesPrivate(colony),
                GetPopulationTaxSolars(colony),

                GetPublicDebt(colony),
                GetAdministrationSalary(colony),

                GetTotal(colony),
            };

            var statistics = new StatisticsResult(
                StatisticCode.SolarDelta,
                $"Детали бюджета",
                fields);
            return new GetStatisticsResult(statistics);
        }

        private static StatisticFieldDto GetIndustriesState(Colony colony)
        {
            var value = colony.GetSolarDeltaIndustries(isPrivate: false);
            return new(
                ParameterCategory.SolarDelta,
                "Госсектор",
                $"{value.ToBeautifulString(setPlus: true)}",
                value.ToStatusByZero(),
                Info: null,
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetIndustriesPrivate(Colony colony)
        {
            var value = colony.GetSolarDeltaIndustries(isPrivate: true);
            return new(
                ParameterCategory.SolarDelta,
                "Частный сектор",
                $"{value.ToBeautifulString(setPlus: true)}",
                value.ToStatusByZero(),
                Info: null,
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetAdministrationSalary(Colony colony)
        {
            var value = colony.GetAdministrationSalary();
            return new(
                ParameterCategory.SolarDelta,
                "Администрация",
                $"{(-value).ToBeautifulString(setPlus: true)}",
                value.ToStatusByZero(invert: true),
                Info: null,
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetPublicDebt(Colony colony)
        {
            var value = colony.State.GetPublicDebt().SolarDelta;
            return new(
                ParameterCategory.SolarDelta,
                "Плата по долгу",
                $"{value.ToBeautifulString(setPlus: true)}",
                value.ToStatusByZero(),
                Info: null,
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetPopulationTaxSolars(Colony colony)
        {
            var value = colony.GetPopulationTaxSolars();
            return new(
                ParameterCategory.SolarDelta,
                "Налог на доходы",
                $"{value.ToBeautifulString(setPlus: true)}",
                value.ToStatusByZero(),
                Info: null,
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetTotal(Colony colony)
        {
            var value = colony.GetSolarDelta();
            return new(
                ParameterCategory.SolarDelta,
                "ИТОГО",
                $"{value.ToBeautifulString(setPlus: true)}",
                value.ToStatusByZero(),
                Info: null,
                ChildrenCode: null);
        }
    }

    public record GetSolarDeltaStatisticsQuery(long UserId) : IRequest<GetStatisticsResult>;
}
