namespace YAGO.World.Host.Controllers.Common.GameRequirements
{
    public record GameRequirementResponse(
        string IconType,
        string Label,
        string Value,
        bool IsMet,
        bool IsLabelFirst,
        string? Url,
        string? InfoUrl);
}
