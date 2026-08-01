using System;
using YAGO.World.Host.Controllers.Episodes;

namespace YAGO.World.Host.Controllers.Events.Models
{
    public record ColonyEventResponse(
        string Id,
        string Title,
        string Type,
        EpisodeResponse Episode,
        bool IsRead,
        DateTime CreatedAtUtc);
}
