using System;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameEvents;

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

        public void SetName(string name)
        {
            Name.SetName(name);
        }

        public void SetChanges(GameEventChangeList changeList, string? stringValue)
        {
            State.SetEpisodeParameters(changeList.ColonyStats);

            if (changeList.StringChange != null && !string.IsNullOrEmpty(stringValue))
                switch (changeList.StringChange)
                {
                    case StringKey.ColonyName:
                        Name.SetName(stringValue);
                        break;
                    default:
                        throw new NotImplementedException();
                }
        }

        public void SetId(long id)
        {
            if (id == Id)
                return;
            if (id != default)
                throw new YagoException("Идентификатор уже установлен.");
            Id = id;
        }

        public void UseTurn(DateTime utcNow)
        {
            TurnReserve.UseTurn(utcNow);
        }
    }
}