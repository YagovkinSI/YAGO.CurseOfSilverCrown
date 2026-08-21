namespace YAGO.World.Host.Controllers.Common.GameVisibleEffects
{
    public record GameVisibleEffectResponse(
        string IconType,
        string Label,
        string Value,
        string Color,
        string? Url,
        string? InfoUrl);
}
