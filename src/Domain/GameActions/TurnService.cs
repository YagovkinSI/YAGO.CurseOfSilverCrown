using YAGO.World.Domain.Colonies;

namespace YAGO.World.Domain.GameActions
{
    public static class TurnService
    {
        public static void SetTurnEndingChanges(this Colony colony)
        {
            var actionPointsDelta = colony.State.Resources.ActionPoints.GetDeltaPerTurn(colony.State);
            colony.State.Resources.ActionPoints.Add(actionPointsDelta);

            var solarsDelta = colony.GetSolarDelta();
            colony.State.Resources.Solars.Add(solarsDelta);

            var moodDelta = colony.State.Resources.Mood.GetDeltaPerTurn(colony.State);
            colony.State.Resources.Mood.Add(moodDelta);

            colony.State.Resources.TurnNumber.Add(1);
        }
    }
}
