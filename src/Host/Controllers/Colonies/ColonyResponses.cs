using System;
using System.Collections.Generic;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;
using YAGO.World.Host.Controllers.Events;

namespace YAGO.World.Host.Controllers.Colonies
{
    public record ColonyPrivate(
        long Id,
        long UserId,
        DateTime NextTurnstartAtUtc,
        string Name,
        IReadOnlyList<ColonyParameterResponse> ColonyParameters,
        IReadOnlyList<ColonyEventSummary> Quests,
        ColonyActionsResponse Actions);

    public record ColonyDetails(
        long Id,
        long UserId,
        string Name,
        IReadOnlyList<ColonyParameterResponse> ColonyParameters);

    public record ColonySummary(
        long Id,
        long UserId,
        string Name);

    public record ColonyActionsResponse(
        bool Reform,
        bool Build);

    public record StatisticsResponse(
        string Title,
        IReadOnlyList<ColonyParameterResponse> ColonyParameters);
}

