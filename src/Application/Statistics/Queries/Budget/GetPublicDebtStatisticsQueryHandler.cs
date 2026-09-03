using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Extensions;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Application.Statistics.Queries.Models;
using YAGO.World.Domain.Colonies.Reforms;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Application.Statistics.Queries.Budget
{
    public class GetPublicDebtStatisticsQueryHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetPublicDebtStatisticsQuery, GetStatisticsResult>
    {
        public async Task<GetStatisticsResult> Handle(
            GetPublicDebtStatisticsQuery query,
            CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(query.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");

            var publicDebt = colony.State.GetPublicDebt();
            var fields = new List<StatisticFieldDto>
            {
                GetPublicDebtValue(publicDebt),
                GetPublicDebtPercent(publicDebt),

                GetTotal(publicDebt),
            };

            var statistics = new StatisticsResult(
                StatisticCode.PublicDebt,
                $"Долг колонии",
                fields);
            return new GetStatisticsResult(statistics);
        }

        private static StatisticFieldDto GetPublicDebtValue(PublicDebt publicDebt)
        {
            var value = publicDebt.Value;
            return new(
                ParameterCategory.Solars,
                "Сумма долга",
                $"{value.ToBeautifulString()}",
                ParameterStatus.Neutral,
                Info: new DisplayInfo(
                    "Сумма долга",
                    description: [
                        "Сумма долга по кредитам колонии."]),
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetPublicDebtPercent(PublicDebt publicDebt)
        {
            var value = publicDebt.InterestRate;
            return new(
                ParameterCategory.Info,
                "Ставка",
                $"{value.ToBeautifulString()}% годовых",
                ParameterStatus.Neutral,
                Info: new DisplayInfo(
                    "Ставка по долгу",
                    description: [
                        "Общая процентаная ставка по кредитам колонии."]),
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetTotal(PublicDebt publicDebt)
        {
            var value = publicDebt.SolarDelta;
            return new(
                ParameterCategory.SolarDelta,
                "ИТОГО",
                $"{value.ToBeautifulString(setPlus: true)}",
                value.ToStatusByZero(),
                Info: new DisplayInfo(
                    "Обслуживание долга",
                    description: [
                        "Сумма выплачиваямая кредитороам по обслуживанию долга."]),
                ChildrenCode: null);
        }
    }

    public record GetPublicDebtStatisticsQuery(long UserId) : IRequest<GetStatisticsResult>;
}
