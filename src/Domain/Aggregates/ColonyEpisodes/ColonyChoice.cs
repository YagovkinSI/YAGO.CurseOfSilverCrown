using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Aggregates.ColonyEpisodes
{
    public class ColonyChoice
    {
        public Choice Choice { get; }
        public ColonyStats ColonyStats { get; }

        public ColonyChoice(Choice choice, ColonyStats colonyStats)
        {
            Choice = choice;
            ColonyStats = colonyStats;
        }

        public (bool IsAvailable, string ButtonName) CheckAvailability()
        {
            return Choice.CheckAvailability(ColonyStats);
        }
    }
}
