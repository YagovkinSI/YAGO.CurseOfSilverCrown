using YAGO.World.Host.Controllers.Episodes;

namespace YAGO.World.Host.Controllers.ColonyActions
{
    public record ColonyActionResponse(
        EpisodeResponse? Episode,
        UpdatedColonyEntities UpdatedEntities);
}
