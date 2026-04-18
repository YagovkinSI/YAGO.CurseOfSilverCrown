using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Aggregates.ColonyEpisodes
{
    public class ColonyEpisode
    {
        public Episode Episode { get; }
        public ColonyStats ColonyStats { get; }

        public ColonyEpisode(Episode episode, ColonyStats colonyStats)
        {
            Episode = episode;
            ColonyStats = colonyStats;
        }

        public IReadOnlyList<ColonyChoice> GetColonyChoices()
        {
            var dilemma = Episode.Dilemma;

            return dilemma switch
            {
                DilemmaSelect dilemmaSelect => dilemmaSelect.Choices
                    .Select(x => new ColonyChoice(x, ColonyStats))
                    .ToList(),
                _ => [],
            };
        }
    }
}
