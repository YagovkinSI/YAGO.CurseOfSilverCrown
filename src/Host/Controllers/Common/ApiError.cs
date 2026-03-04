namespace YAGO.World.Host.Controllers.Common
{
    public record ApiError(
        string Code,
        string Message,
        string? Details);
}
