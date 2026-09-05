using System.Collections.Generic;
using System.Linq;
using YAGO.World.Application.Council;
using YAGO.World.Application.Council.Queries.GetCouncilPositions;
using YAGO.World.Domain.Persons;

namespace YAGO.World.Host.Controllers.Council
{
    public static class CouncilResponseMapping
    {
        public static IReadOnlyList<CouncilPositionResponse> ToResponse(
            this IEnumerable<CouncilPositionDto> positions) =>
            positions.Select(position => new CouncilPositionResponse(
                position.Code.ToResponse(),
                position.Title,
                position.Description,
                position.CanHire,
                position.Person?.ToResponse(position.Loyalty))).ToList();

        private static CouncilMemberResponse? ToResponse(this Person person, int loyalty) =>
            new CouncilMemberResponse(
                person.Name,
                person.Avatar,
                loyalty,
                person.WikiArticleCode);

        private static string ToResponse(this CouncilPosition councilPosition)
        {
            return councilPosition switch
            {
                CouncilPosition.Administrator => CouncilPositionCodeConstants.Administrator,
                CouncilPosition.Engineer => CouncilPositionCodeConstants.Engineer,
                CouncilPosition.Financier => CouncilPositionCodeConstants.Financier,
                CouncilPosition.Social => CouncilPositionCodeConstants.Social,
            };
        }
    }
}