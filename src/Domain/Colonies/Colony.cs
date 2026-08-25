using System;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Colonies
{
    public class Colony : IEntity<long>
    {
        public long Id { get; private set; }
        public long UserId { get; }
        public ColonyDisplayInfo DisplayInfo { get; private set; }
        public ColonyState State { get; }

        public Colony(
            long id,
            long userId,
            ColonyDisplayInfo name,
            ColonyState stats)
        {
            Id = id;
            UserId = userId;
            DisplayInfo = name;
            State = stats;
        }

        public static Colony CreateNew(long userId)
        {
            var name = ColonyDisplayInfo.CreateNew();
            var colonyStats = ColonyState.CreateNew();
            return new Colony(
                id: default,
                userId: userId,
                name: name,
                colonyStats);
        }

        public void SetName(string? name)
        {
            DisplayInfo.SetName(name);
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
            State.TurnReserve.UseTurn(utcNow);
        }
    }
}