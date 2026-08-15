using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Dataset.Prologue;
using YAGO.World.Domain.Turns;

namespace YAGO.World.Domain.Colonies
{
    public class Colony : IEntity<long>
    {
        public long Id { get; private set; }
        public long UserId { get; }
        public TurnReserve TurnReserve { get; }
        public ColonyName Name { get; private set; }
        public ColonyState State { get; }
        public IReadOnlyDictionary<string, ColonyEvent> Events => _events;
        private readonly Dictionary<string, ColonyEvent> _events;

        public Colony(
            long id,
            long userId,
            TurnReserve turnReserve,
            ColonyName name,
            ColonyState stats,
            IEnumerable<ColonyEvent> events)
        {
            Id = id;
            UserId = userId;
            TurnReserve = turnReserve;
            Name = name;
            State = stats;
            _events = events.ToDictionary(x => x.EventId);
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

        public void RemoveEvent(string id)
        {
            _events.Remove(id);
        }

        public void AddEvents(IReadOnlyList<string> newEvents)
        {
            foreach (var eventId in newEvents)
            {
                if (_events.ContainsKey(eventId))
                    continue;
                var colonyEvent = ColonyEvent.CreateNew(eventId);
                _events.Add(colonyEvent.EventId, colonyEvent);
            }
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