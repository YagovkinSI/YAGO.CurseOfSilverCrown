using System.Linq;
using YAGO.World.Application.Statistics.Queries.Models;

namespace YAGO.World.Host.Controllers.Statistics
{
    public static class StatisticsResponseMapping
    {
        public static StatisticsResponse ToResponse(
            this StatisticsDto statisticsDto)
        {
            var parameters = statisticsDto.Fields
                .Select(x => x.ToResponse())
                .ToList();

            return new StatisticsResponse(
                statisticsDto.Code.ToResponse(),
                statisticsDto.Title,
                parameters);
        }

        public static StatisticFieldResponse ToResponse(
            this StatFieldDto statFieldDto)
        {
            return new StatisticFieldResponse(
                statFieldDto.Category.ToResponse(),
                statFieldDto.Label,
                statFieldDto.Value,
                statFieldDto.Status.ToResponse(),
                statFieldDto.Description,
                statFieldDto.ChildrenCode?.ToResponse());
        }

        private static string ToResponse(this ParameterStatus parameterStatus)
        {
            return parameterStatus switch
            {
                ParameterStatus.Critical => ParameterStatusConstants.Critical,
                ParameterStatus.Bad => ParameterStatusConstants.Bad,
                ParameterStatus.Neutral => ParameterStatusConstants.Neutral,
                ParameterStatus.Good => ParameterStatusConstants.Good,
                ParameterStatus.Excellent => ParameterStatusConstants.Excellent,
            };
        }

        private static string ToResponse(this StatisticCode statisticCode)
        {
            return statisticCode switch
            {
                StatisticCode.Main => StatisticCodeConstants.Main,
                StatisticCode.SolarDelta => StatisticCodeConstants.SolarDelta,
            };
        }

        private static string ToResponse(this StatisticCategory statisticCategory)
        {
            return statisticCategory switch
            {
                StatisticCategory.Info => StatisticCategoryConstants.Info,
                StatisticCategory.Solars => StatisticCategoryConstants.Solars,
            };
        }
    }
}
