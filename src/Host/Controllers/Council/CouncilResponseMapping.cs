using System.Collections.Generic;
using System.Linq;
using YAGO.World.Application.Council.Queries.GetCouncilPositions;

namespace YAGO.World.Host.Controllers.Council
{
    public static class CouncilResponseMapping
    {
        public static IReadOnlyList<CouncilPositionResponse> ToResponse(
            this IEnumerable<CouncilPositionDto> positions) =>
            positions.Select(position => new CouncilPositionResponse(
                position.Code,
                position.Title,
                position.Description,
                position.CanHire,
                position.Member?.ToResponse())).ToList();

        private static CouncilMemberResponse? ToResponse(this CouncilMemberDto? member) =>
            member == null
                ? null
                : new CouncilMemberResponse(
                    member.Name,
                    member.Avatar,
                    member.Loyalty,
                    member.WikiArticleCode);
    }
}