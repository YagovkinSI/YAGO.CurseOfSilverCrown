using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Extensions;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Application.Statistics.Queries.Models;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Stations;
using YAGO.World.Domain.Services;

namespace YAGO.World.Application.Statistics.Queries.More
{
    public class GetAsteroidStatisticsQueryHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetAsteroidStatisticsQuery, GetStatisticsResult>
    {
        public async Task<GetStatisticsResult> Handle(
            GetAsteroidStatisticsQuery query,
            CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(query.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");
            var asteroid = colony.State.GetAsteroid()
                ?? throw new YagoException("Астероид ещё не выбран.");

            var fields = new List<StatisticFieldDto>
            {
                GetFieldName(asteroid),
                GetFieldDomain(),
                GetFieldCanton(),
                GetFieldType(),
                GetFieldDistanceToCeres(asteroid),
                GetFieldDistanceToAvalon(asteroid),
                GetFieldMiningModulesLimit(asteroid)
            };

            var statistics = new StatisticsResult(
                StatisticCode.Asteroid,
                "Астероид",
                fields);
            return new GetStatisticsResult(statistics);
        }

        private static StatisticFieldDto GetFieldName(Asteroid asteroid)
        {
            return new(
                ParameterCategory.Info,
                "Название",
                asteroid.Name,
                ParameterStatus.Neutral,
                Info: new DisplayInfo(
                    "Название",
                    description: [
                        "Название выбранного астероида."]),
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldDomain()
        {
            return new(
                ParameterCategory.Info,
                "Домен",
                "Церера",
                ParameterStatus.Neutral,
                Info: new DisplayInfo(
                    "Домен",
                    description: [
                        "Государственное образование, к которому относится кантон."]),
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldCanton()
        {
            return new(
                ParameterCategory.Info,
                "Кантон",
                "Авалон",
                ParameterStatus.Neutral,
                Info: new DisplayInfo(
                    "Кантон",
                    description: [
                        "Регион Пояса астероидов, в котором расположен астероид."]),
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldType()
        {
            return new(
                ParameterCategory.Info,
                "Тип",
                "Платиноид",
                ParameterStatus.Neutral,
                Info: new DisplayInfo(
                    "Тип",
                    description: [
                        "Тип астероида. Платиноиды богаты редкоземельными металлами группы платины."]),
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldDistanceToCeres(Asteroid asteroid)
        {
            return new(
                ParameterCategory.Info,
                "Расстояние до Цереры",
                $"{asteroid.DistanceToCeres.ToBeautifulString()} а.е.",
                ParameterStatus.Neutral,
                Info: new DisplayInfo(
                    "Расстояние до Цереры",
                    description: [
                        "Расстояние от астероида до Цереры в астрономических единицах."]),
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldDistanceToAvalon(Asteroid asteroid)
        {
            return new(
                ParameterCategory.Info,
                "Расстояние до Авалона",
                $"{asteroid.DistanceToAvalon.ToBeautifulString()} а.е.",
                ParameterStatus.Neutral,
                Info: new DisplayInfo(
                    "Расстояние до Авалона",
                    description: [
                        "Расстояние от астероида до станции «Авалон» в астрономических единицах."]),
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldMiningModulesLimit(Asteroid asteroid)
        {
            return new(
                ParameterCategory.Info,
                "Лимит добывающих модулей",
                $"{asteroid.MiningModulesLimit.ToBeautifulString()}",
                ParameterStatus.Neutral,
                Info: new DisplayInfo(
                    "Лимит добывающих модулей",
                    description: [
                        "Максимальное число добывающих модулей, которое можно построить на астероиде."]),
                ChildrenCode: null);
        }
    }

    public record GetAsteroidStatisticsQuery(long UserId) : IRequest<GetStatisticsResult>;
}