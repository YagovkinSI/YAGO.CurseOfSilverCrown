using System;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Colonies
{
    public class Colony : IEntity<long>
    {
        public long Id { get; private set; }
        public long UserId { get; }
        public ColonyState State { get; }

        public string DisplayName => State.DisplayName;

        public Colony(
            long id,
            long userId,
            ColonyState stats)
        {
            Id = id;
            UserId = userId;
            State = stats;
        }

        public static Colony CreateNew(long userId)
        {
            var colonyStats = ColonyState.CreateNew();
            return new Colony(
                id: default,
                userId: userId,
                colonyStats);
        }

        public void SetName(string? name)
        {
            State.Name.SetName(name);
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