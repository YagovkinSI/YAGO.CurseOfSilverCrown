namespace YAGO.World.Host.Controllers.Episodes
{
    public record SlideButtonResponse(
        string? Name,
        bool IsAvailable,
        SlideButtonActionResponse? Action,
        SlideButtonNavigateResponse? Navigate,
        SlideButtonToSlideResponse? ToSlide);
}
