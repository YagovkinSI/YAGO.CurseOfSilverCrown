using System;
using System.Linq;
using YSI.CurseOfSilverCrown.Web.Database.Commands;
using YSI.CurseOfSilverCrown.Web.Database.Domains;
using YSI.CurseOfSilverCrown.Web.Database.Routes;
using YSI.CurseOfSilverCrown.Web.Database.Turns;
using YSI.CurseOfSilverCrown.Web.Database.Units;
using YSI.CurseOfSilverCrown.Web.Parameters;

namespace YSI.CurseOfSilverCrown.Web.Helpers.StartingDatas
{
    internal static class StartingData
    {
        private static readonly Turn firstTurn = new()
        {
            Id = 1,
            Started = DateTime.MinValue,
            IsActive = true
        };

        public static Domain[] Domains =>
            StartingDataMap.Array
                .Select(p => new Domain
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
