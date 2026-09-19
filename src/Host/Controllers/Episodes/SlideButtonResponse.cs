namespace YAGO.World.Host.Controllers.Episodes
{
    public record SlideButtonResponse(
        string? Name,
        bool IsAvailable,
        SlideButtonActionResponse? Action,
        SlideButtonNavigateResponse? Navigate,
        SlideButtonToSlideResponse? ToSlide,
        string? InfoSlideId,
        string? AdministratorSlideId,
        string? EngineerSlideId,
        string? FinancierSlideId,
        string? SocialSlideId,
        string Kind);
}
