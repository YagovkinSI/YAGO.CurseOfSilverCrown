namespace YAGO.World.Host.Controllers.Common
{
    public record DisplayInfoResponse(
        string Name,
        string? ImageName,
        string[] Description);
}
