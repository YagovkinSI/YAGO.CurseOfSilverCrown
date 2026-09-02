using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Extensions;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Application.Ratings.Models;
using YAGO.World.Application.Statistics.Queries.Models;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Slots;

namespace YAGO.World.Application.Ratings.Queries
{
    public class GetRatingsQueryHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetRatingsQuery, List<StatisticFieldDto>>
    {
        public async Task<List<StatisticFieldDto>> Handle(
            GetRatingsQuery query,
            CancellationToken cancellationToken)
        {
            var colonies = await colonyRepository.GetPaginatedColonies(1, 50, cancellationToken);

            return colonies.Data
                .OrderByDescending(colony => GetSortKey(colony, query.Code))
                .Take(10)
                .Select(colony => GetRatingField(colony, query.Code))
                .ToList();
        }

        private static double GetSortKey(Colony colony, RatingCode code) => code switch
        {
            RatingCode.Population => colony.State.GetPopulation(),
            RatingCode.Laws => colony.State.Reforms[ColonyReformType.SocialGuaranteesLevel].Value -
                colony.State.Reforms[ColonyReformType.TaxLevel].Value,
            RatingCode.Mood => colony.State.Resources.Mood.Value,
            RatingCode.Budget => colony.GetSolarDelta(),
            RatingCode.Area => colony.State.Slots[ColonySlotType.Modules].GetUsed(colony.State),
            RatingCode.Week => colony.State.Resources.TurnNumber.Value,
        };

        private static StatisticFieldDto GetRatingField(Colony colony, RatingCode code) => code switch
        {
            RatingCode.Population => GetPopulationField(colony),
            RatingCode.Laws => GetLawsField(colony),
            RatingCode.Mood => GetMoodField(colony),
            RatingCode.Budget => GetBudgetField(colony),
            RatingCode.Area => GetAreaField(colony),
            RatingCode.Week => GetWeekField(colony),
        };

        private static StatisticFieldDto GetPopulationField(Colony colony)
        {
            var value = $"{colony.State.GetPopulation().ToBeautifulString()} чел.";
            return BuildField(colony, ParameterCategory.Population, value, ParameterStatus.Neutral);
        }

        private static StatisticFieldDto GetLawsField(Colony colony)
        {
            var humanism = colony.State.Reforms[ColonyReformType.SocialGuaranteesLevel].Value -
                colony.State.Reforms[ColonyReformType.TaxLevel].Value;
            var result = humanism switch
            {
                > 1 => "Гуманные",
                < -1 => "Корпоративные",
                _ => "Стандартные"
            };
            return BuildField(colony, ParameterCategory.Reforms, result, ParameterStatus.Neutral);
        }

        private static StatisticFieldDto GetMoodField(Colony colony)
        {
            var mood = colony.State.Resources.Mood.Value;
            var status = mood > 40 ? ParameterStatus.Neutral : ParameterStatus.Bad;
            return BuildField(colony, ParameterCategory.Mood, mood.ToBeautifulString(), status);
        }

        private static StatisticFieldDto GetBudgetField(Colony colony)
        {
            var delta = colony.GetSolarDelta();
            var status = delta > 0 ? ParameterStatus.Good : ParameterStatus.Bad;
            var value = $"{delta.ToBeautifulString(setPlus: true)} солар/ход";
            return BuildField(colony, ParameterCategory.SolarDelta, value, status);
        }

        private static StatisticFieldDto GetAreaField(Colony colony)
        {
            var slots = colony.State.Slots[ColonySlotType.Modules];
            var value = $"{slots.GetUsed(colony.State).ToBeautifulString()}/{slots.GetTotal(colony.State).ToBeautifulString()}";
            return BuildField(colony, ParameterCategory.Info, value, ParameterStatus.Neutral);
        }

        private static StatisticFieldDto GetWeekField(Colony colony)
        {
            var value = colony.State.Resources.TurnNumber.Value.ToBeautifulString();
            return BuildField(colony, ParameterCategory.Info, value, ParameterStatus.Neutral);
        }

        private static StatisticFieldDto BuildField(
            Colony colony,
            ParameterCategory category,
            string value,
            ParameterStatus status)
        {
            return new StatisticFieldDto(
                category,
                colony.DisplayInfo.DisplayName,
                value,
                status,
                Info: null,
                ChildrenCode: null);
        }
    }

    public record GetRatingsQuery(RatingCode Code, long? UserId) : IRequest<List<StatisticFieldDto>>;
}
