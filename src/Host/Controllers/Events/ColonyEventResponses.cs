using System;
using YAGO.World.Host.Controllers.Episodes;

namespace YAGO.World.Host.Controllers.Events
{
    public record ColonyEventPrivate(
        long Id,
        string Title,
        string Type,
        EpisodeResponse Episode,
        bool IsRead,
        DateTime CreatedAtUtc);

    public record ColonyEventSummary(
        long Id,
        string Title,
        string Type,
        bool IsRead,
        DateTime CreatedAtUtc);
}
