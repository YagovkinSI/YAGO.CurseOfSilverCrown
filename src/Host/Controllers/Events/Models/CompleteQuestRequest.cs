namespace YAGO.World.Host.Controllers.Events.Models
{
    public record CompleteQuestRequest(
        long ColonyEventId,
        string DilemmaResolving);
}
