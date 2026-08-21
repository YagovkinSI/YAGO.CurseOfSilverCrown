using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Application.GameActions
{
    public interface IApplyGameActionService
    {
        GameActionResultDto Apply(
            GameAction gameAction,
            Colony colony,
            string? stringValue = null);
    }

    public class ApplyGameActionService : IApplyGameActionService
    {
        public GameActionResultDto Apply(
            GameAction gameAction,
            Colony colony,
            string? stringValue = null)
        {
            var eventResult = GameActionResult.CreateNew(gameAction.DisplayInfoResult);
            eventResult.SetMainParametersBefore(colony);
            gameAction.Aplly(colony, stringValue);
            eventResult.SetMainParametersAfter(colony);

            var turnNumber = colony.State.Resources.TurnNumber.Value;
            var newColonyEvents = gameAction.NewEventCodes
                .Select(x => ColonyEvent.CreateNew(colony.Id, x, turnNumber))
                .ToList();

            return new GameActionResultDto(eventResult, newColonyEvents);
        }
    }

    public record GameActionResultDto(GameActionResult GameActionResult, IReadOnlyList<ColonyEvent> NewColonyEvents);
}
