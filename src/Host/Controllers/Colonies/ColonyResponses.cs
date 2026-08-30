using System;
using System.Collections.Generic;
using YAGO.World.Host.Controllers.Events;

namespace YAGO.World.Host.Controllers.Colonies
{
    public record ColonyPrivate(
        long Id,
        long UserId,
        DateTime NextTurnstartAtUtc,
        string Name,
        IReadOnlyList<ColonyEventSummary> Quests,
        ColonyActionsResponse Actions);

    public record ColonyActionsResponse(
        bool Reform,
        bool Build,
        bool Statistics);
}

