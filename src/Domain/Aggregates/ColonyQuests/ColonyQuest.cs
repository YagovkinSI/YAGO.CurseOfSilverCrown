using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Aggregates.ColonyEpisodes;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Aggregates.ColonyQuests
{
    public class ColonyQuest
    {
        public GameEvent GameEvent { get; }
        public ColonyStats ColonyStats { get; }
        public string Progress { get; }
        public bool Completed { get; }

        public ColonyQuest(
            ColonyStats colonyStats,
            GameEvent gameEvent)
        {
            GameEvent = gameEvent;
            ColonyStats = colonyStats;
            (Progress, Completed) = GetProgress(colonyStats, gameEvent);
        }

        public ColonyEpisode GetPrologueColonyEpisode()
        {
            return new ColonyEpisode(GameEvent.Episode, ColonyStats);
        }

        private (string progress, bool completed) GetProgress(ColonyStats colonyStats, GameEvent gameEvent)
        {
            var progress = new Dictionary<string, bool>();
            var completed = true;
            foreach (var parameter in gameEvent.Episode.Slides.SelectMany(x => x.Parameters))
            {
                var stat = colonyStats.GetGameParameter(parameter.Name);
                if (!progress.ContainsKey(parameter.Name))
                    progress.Add(parameter.Name, stat >= parameter.Value);
                if (stat < parameter.Value)
                    completed = false;
            }
            var progressString = progress.Count == 0
                ? "-"
                : $"{progress.Values.Count(x => x)}/{progress.Values.Count}";
            return (progressString, completed);
        }
    }
}
