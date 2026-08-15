using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Dataset.Prologue;

namespace YAGO.World.Domain.Colonies
{
    public class Colony : IEntity<long>
    {
        public long Id { get; private set; }
        public long UserId { get; }
        public TurnReserve TurnReserve { get; }
        public ColonyName Name { get; private set; }
        public ColonyState State { get; }
        public IReadOnlyList<ColonyEvent> Events { get; private set; }

        public Colony(
            long id,
            long userId,
            TurnReserve turnReserve,
            ColonyName name,
            ColonyState stats,
            IReadOnlyList<ColonyEvent> events)
        {
            Id = id;
            UserId = userId;
            TurnReserve = turnReserve;
            Name = name;
            State = stats;
            Events = events;
        }

        public static Colony CreateNew(long userId)
        {
            var turnReserve = TurnReserve.CreateNew();
            var name = ColonyName.CreateNew();
            var colonyStats = ColonyState.CreateNew();
            var startEvent = ColonyEvent.CreateNew(nameof(ColonyNameEvent));
            return new Colony(
                id: default,
                userId: userId,
                turnReserve,
                name: name,
                colonyStats,
                events: [startEvent]);
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

        public void SetId(long id)
        {
            if (id == Id)
                return;
            if (id != default)
                throw new YagoException("Идентификатор уже установлен.");
            Id = id;
        }

        public void UseTurn(DateTime utcNow) => TurnReserve.UseTurn(utcNow);
    }
}