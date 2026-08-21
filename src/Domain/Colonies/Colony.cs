using System;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Colonies
{
    public class Colony : IEntity<long>
    {
        public long Id { get; private set; }
        public long UserId { get; }
        public TurnReserve TurnReserve { get; }
        public ColonyName Name { get; private set; }
        public ColonyState State { get; }

        public Colony(
            long id,
            long userId,
            TurnReserve turnReserve,
            ColonyName name,
            ColonyState stats)
        {
            Id = id;
            UserId = userId;
            TurnReserve = turnReserve;
            Name = name;
            State = stats;
        }

        public static Colony CreateNew(long userId)
        {
            var turnReserve = TurnReserve.CreateNew();
            var name = ColonyName.CreateNew();
            var colonyStats = ColonyState.CreateNew();
            return new Colony(
                id: default,
                userId: userId,
                turnReserve,
                name: name,
                colonyStats);
        }

        public void SetName(string? name)
        {
            Name.SetName(name);
        }

        public void SetId(long id)
        {
            if (id == Id)
                return;
            if (Id != default)
                throw new YagoException("Идентификатор уже установлен.");
            Id = id;
        }

        public void UseTurn(DateTime utcNow)
        {
            TurnReserve.UseTurn(utcNow);
        }

        public void SetTurnEndingChanges()
        {
            var actionPointsDelta = State.Resources.ActionPoints.GetDeltaPerTurn(State);
            State.Resources.ActionPoints.Add(actionPointsDelta);

            var solarsDelta = State.Resources.Solars.GetDeltaPerTurn(State);
            State.Resources.Solars.Add(solarsDelta);

            var moodDelta = State.Resources.Mood.GetDeltaPerTurn(State);
            State.Resources.Mood.Add(moodDelta);

            State.Resources.TurnNumber.Add(1);
        }
    }
}