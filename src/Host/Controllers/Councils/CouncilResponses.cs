namespace YAGO.World.Host.Controllers.Councils
{
    public record CouncilMemberResponse(
        string Name,
        string Avatar,
        int Loyalty,
        string WikiArticleCode);

    public record CouncilPositionResponse(
        string Code,
        string Title,
        string Description,
        long? HireEventId,
        CouncilMemberResponse? Member);
}
