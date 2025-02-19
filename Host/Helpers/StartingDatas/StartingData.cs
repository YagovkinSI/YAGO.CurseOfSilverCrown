using System;
using System.Linq;
using YAGO.World.Host.Database.Commands;
using YAGO.World.Host.Database.Domains;
using YAGO.World.Host.Database.Routes;
using YAGO.World.Host.Database.Turns;
using YAGO.World.Host.Database.Units;
using YAGO.World.Host.Parameters;

namespace YAGO.World.Host.Helpers.StartingDatas
{
    internal static class StartingData
    {
        private static readonly Turn firstTurn = new()
        {
            Id = 1,
            Started = DateTime.MinValue,
            IsActive = true
        };

        public static Organization[] Domains =>
            StartingDataMap.Array
                .Select(p => new Organization
                {
                    Id = p.Id,
                    Name = p.Name,
                    SuzerainId = p.SuzerainId,
                    Size = p.Size,
                    TurnOfDefeat = int.MinValue,
                    Gold = RandomHelper.AddRandom(CoffersParameters.StartCount * ((p.VassalCount / 3) + 1),
                        randomNumber: RandomHelper.DependentRandom(p.Id, 1), roundRequest: -1),
                    Fortifications = RandomHelper.AddRandom(p.Fortifications, randomNumber: RandomHelper.DependentRandom(p.Id, 2), roundRequest: -1),
                    Investments = RandomHelper.AddRandom(p.Investments, randomNumber: RandomHelper.DependentRandom(p.Id, 3), roundRequest: -1)
                })
                .ToArray();

        public static Unit[] Units =>
            StartingDataMap.Array
                .Select(p => new Unit
                {
                    Id = p.Id,
                    DomainId = p.Id,
                    PositionDomainId = p.Id,
                    Warriors = RandomHelper.AddRandom(p.Warrioirs,
                        randomNumber: RandomHelper.DependentRandom(p.Id, 0)),
                    Type = UnitCommandType.WarSupportDefense,
                    TargetDomainId = p.Id,
                    Status = CommandStatus.ReadyToMove
                })
                .ToArray();

        internal static Turn GetFirstTurn() => firstTurn;

        internal static Route[] Routes =>
            StartingDataMap.Array
                .SelectMany(b => b.BorderingDomainModelIds
                    .Select(r => new Route
                    {
                        FromDomainId = b.Id,
                        ToDomainId = r
                    }))
                .ToArray();
    }
}
