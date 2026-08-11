using System;

namespace YAGO.World.Host.Controllers.Turns
{
    public record MyTurn(
        Guid Id,
        Guid ColonyId,
        DateTime StartAtUtc,
        DateTime? RunAtUtc);
}
