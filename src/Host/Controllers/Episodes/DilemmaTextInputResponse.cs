namespace YAGO.World.Host.Controllers.Episodes
{
    public record DilemmaTextInputResponse(
        SlideResponse Slide,
        string SubmitButtonName)
        : DilemmaResponse(Domain.Entities.Episodes.DilemmaType.TextInput.ToString());
}
