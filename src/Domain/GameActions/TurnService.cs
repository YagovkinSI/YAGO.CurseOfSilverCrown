using YAGO.World.Domain.Colonies;

namespace YAGO.World.Domain.GameActions
{
    public static class TurnService
    {
        public static void SetTurnEndingChanges(this Colony colony)
        {
            var actionPointsDelta = colony.GetActionPointsDelta();
            colony.State.Resources.ActionPoints.Add(actionPointsDelta);

            var solarsDelta = colony.GetSolarDelta();
            colony.State.Resources.Solars.Add(solarsDelta);

            var moodDelta = colony.State.Mood.GetDeltaPerTurn(colony.State);
            colony.State.Mood.Add(moodDelta);

            colony.State.TurnNumber.Add(1);
        }
    }
}
