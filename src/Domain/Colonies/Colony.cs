using System;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Colonies
{
    public class Colony : IEntity<long>
    {
        public long Id { get; private set; }
        public long UserId { get; }
        public ColonyName Name { get; }
        public ColonyState State { get; }

        public string DisplayName => Name.Named
            ? Name.DatabaseName
            : "Колония";

        public Colony(
            long id,
            long userId,
            ColonyName name,
            ColonyState stats)
        {
            Id = id;
            UserId = userId;
            Name = name;
            State = stats;
        }

        public static Colony CreateNew(long userId)
        {
            var colonyStats = ColonyState.CreateNew();
            var name = ColonyName.CreateNew();
            return new Colony(
                id: default,
                userId: userId,
                name,
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
            State.TurnReserve.UseTurn(utcNow);
        }
    }
}