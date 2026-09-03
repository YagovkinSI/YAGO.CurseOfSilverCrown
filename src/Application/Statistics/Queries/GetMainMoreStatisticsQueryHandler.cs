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

namespace YAGO.World.Application.Statistics.Queries
{
    public class GetMainMoreStatisticsQueryHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetMainMoreStatisticsQuery, GetStatisticsResult>
    {
        public async Task<GetStatisticsResult> Handle(
            GetMainMoreStatisticsQuery query,
            CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(query.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");

            var fields = new List<StatisticFieldDto>
            {
                GetFieldStation(colony),

                GetFieldGdp(colony),
                GetFieldPopulation(colony),

                GetFieldReforms(colony),

                GetFieldTurnNumber(colony),
            };

            var statistics = new StatisticsResult(
                StatisticCode.MainMore,
                $"Дополнительная информация",
                fields);
            return new GetStatisticsResult(statistics);
        }

        private static StatisticFieldDto GetFieldStation(Colony colony)
        {
            return new(
                ParameterCategory.Info,
                "Станция",
                "Рассвет-342",
                ParameterStatus.Neutral,
                Info: new DisplayInfo(
                    "Станция",
                    description: [
                        "Модель станции."]),
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldGdp(Colony colony)
        {
            return new(
                ParameterCategory.Solars,
                "ВВП",
                $"{colony.State.GetGdp().ToBeautifulString()} солар",
                ParameterStatus.Neutral,
                Info: new DisplayInfo(
                    "ВВП",
                    description: [
                        "Суммарная стоимость товаров и услуг, произведённых колонией за один год."]),
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldPopulation(Colony colony)
        {
            return new(
                ParameterCategory.Population,
                "Население",
                $"{colony.State.GetPopulation().ToBeautifulString()}",
                ParameterStatus.Neutral,
                Info: new DisplayInfo(
                    "Население",
                    description: [
                        "Число жителей колонии."]),
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldReforms(Colony colony)
        {
            var humanism = colony.State.Reforms[ColonyReformType.SocialGuaranteesLevel].Value -
                colony.State.Reforms[ColonyReformType.TaxLevel].Value;
            var value = humanism switch
            {
                > 1 => "Гуманные",
                < -1 => "Корпоративные",
                _ => "Стандартные"
            };
            return new(
                ParameterCategory.Reforms,
                "Законы",
                value,
                ParameterStatus.Neutral,
                Info: new DisplayInfo(
                    "Законы",
                    description: [
                        "Характер законодательства: гуманный, стандартный или корпоративный — в зависимости от баланса реформ."]),
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldTurnNumber(Colony colony)
        {
            return new(
                ParameterCategory.Info,
                "Ход",
                $"{colony.State.Resources.TurnNumber.Value.ToBeautifulString()}",
                ParameterStatus.Neutral,
                Info: new DisplayInfo(
                    "Ход",
                    description: [
                        "Текущий номер хода игры."]),
                ChildrenCode: null);
        }

    }

    public record GetMainMoreStatisticsQuery(long UserId) : IRequest<GetStatisticsResult>;
}
