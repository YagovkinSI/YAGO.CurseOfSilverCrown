namespace YAGO.World.Host.Controllers.Council
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
        bool CanHire,
        CouncilMemberResponse? Member);
}