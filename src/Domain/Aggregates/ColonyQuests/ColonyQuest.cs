using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Aggregates.ColonyEpisodes;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Quests;

namespace YAGO.World.Domain.Aggregates.ColonyQuests
{
    public class ColonyQuest
    {
        public Quest Quest { get; }
        public ColonyStats ColonyStats { get; }
        public string Progress { get; }
        public bool Completed { get; }

        public ColonyQuest(
            ColonyStats colonyStats,
            Quest quest)
        {
            Quest = quest;
            ColonyStats = colonyStats;
            (Progress, Completed) = GetProgress(colonyStats, quest);
        }

        public ColonyEpisode GetPrologueColonyEpisode()
        {
            return new ColonyEpisode(Quest.PrologueEpisode, ColonyStats);
        }

        private (string progress, bool completed) GetProgress(ColonyStats colonyStats, Quest quest)
        {
            var progress = new Dictionary<string, bool>();
            var completed = true;
            foreach (var parameter in quest.PrologueEpisode.Slides.SelectMany(x => x.Parameters))
            {
                var stat = colonyStats.GetGameParameter(parameter.Name);
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
