using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Dataset.Prologue;
using YAGO.World.Domain.Turns;

namespace YAGO.World.Domain.Colonies
{
    public class Colony : IEntity<Guid>
    {
        public Guid Id { get; }
        public long UserId { get; }
        public ColonyName Name { get; private set; }
        public ColonyState State { get; }
        public IReadOnlyList<ColonyEvent> Events { get; private set; }

        public Colony(
            Guid id,
            long userId,
            ColonyName name,
            ColonyState stats,
            IReadOnlyList<ColonyEvent> events)
        {
            Id = id;
            UserId = userId;
            Name = name;
            State = stats;
            Events = events;
        }

        public static IReadOnlyList<IEntity> CreateNew(long userId)
        {
            var colonyId = Guid.NewGuid();
            var name = ColonyName.CreateNew();
            var colonyStats = ColonyState.CreateNew(colonyId);
            var startEvent = ColonyEvent.CreateNew(nameof(ColonyNameEvent));
            var colony = new Colony(
                colonyId,
                userId: userId,
                name: name,
                colonyStats,
                events: [startEvent]);
            var turn = Turn.CreateNew(
                colony.Id,
                prevTurn: null);
            return [colony, turn];
        }

        public void SetName(string name)
        {
            Name.SetName(name);
        }

        public bool IsNewColonyAvailable()
        {
            return Events.Count == 0;
        }

        public void RemoveEvent(string id)
        {
            var list = Events.ToList();
            var removingQuest = list.Single(x => x.EventId == id);
            list.Remove(removingQuest);
            Events = list;
        }

        public void AddEvents(IReadOnlyList<string> newEvents)
        {
            var list = new List<ColonyEvent>(newEvents.Count);
            foreach (var eventId in newEvents)
            {
                if (Events.Any(x => x.EventId == eventId))
                    continue;
                var colonyEvent = ColonyEvent.CreateNew(eventId);
                list.Add(colonyEvent);
            }

            Events = [
                ..Events,
                ..list];
        }

        public void SetChanges(GameEventChangeList changeList)
        {
            State.SetEpisodeParameters(changeList.ColonyStats);
            AddEvents(changeList.NewQuests);
        }
    }
}