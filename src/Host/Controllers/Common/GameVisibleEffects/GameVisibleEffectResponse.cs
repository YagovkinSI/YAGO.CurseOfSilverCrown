namespace YAGO.World.Host.Controllers.Common.GameVisibleEffects
{
    public record GameVisibleEffectResponse(
        string IconType,
        string Label,
        string Value,
        bool Status,
        string? Url,
        string? InfoUrl);
}
