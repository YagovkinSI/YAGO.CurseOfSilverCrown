using System;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.MainModels.Characters;
using YSI.CurseOfSilverCrown.Core.MainModels.Commands;
using YSI.CurseOfSilverCrown.Core.MainModels.Commands.UnitCommands;
using YSI.CurseOfSilverCrown.Core.MainModels.Domains;
using YSI.CurseOfSilverCrown.Core.MainModels.Routes;
using YSI.CurseOfSilverCrown.Core.MainModels.Turns;
using YSI.CurseOfSilverCrown.Core.MainModels.Units;
using YSI.CurseOfSilverCrown.Core.MainModels.Sessions;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Utils;

namespace YSI.CurseOfSilverCrown.Core.MainModels
{
    internal static class StartingData
    {
        private static readonly Turn firstTurn = new Turn
        {
            Id = 1,
            Started = DateTime.MinValue,
            IsActive = true
        };

        private static readonly Session firstGameSession = new Session
        {
            Id = 1,
            EndSeesionTurnId = int.MaxValue,
            StartSeesionTurnId = 1,
            NumberOfGame = 1
        };

        public static Domain[] Organizations =>
            StartingDataMap.Array
                .Select(p => new Domain
                {
                    Id = p.Id,
                    Name = p.Name,
                    MoveOrder = p.Size,
                    TurnOfDefeat = int.MinValue,
                    Coffers = RandomHelper.AddRandom(CoffersParameters.StartCount, randomNumber: RandomHelper.DependentRandom(p.Id, 1), roundRequest: -1),
                    Fortifications = RandomHelper.AddRandom(FortificationsParameters.StartCount, randomNumber: RandomHelper.DependentRandom(p.Id, 2), roundRequest: -1),
                    PersonId = p.Id,
                    Investments = RandomHelper.AddRandom(InvestmentsHelper.StartInvestment, randomNumber: RandomHelper.DependentRandom(p.Id, 3), roundRequest: -1)
                })
                .ToArray();

        public static Character[] Persons =>
            StartingDataMap.Array
                .Select(p => new Character
                {
                    Id = p.Id,
                    Name = "Эйгон " + p.Id.ToString()
                })
                .ToArray();

        public static Unit[] Units =>
            StartingDataMap.Array
                .Select(p => new Unit
                {
                    Id = p.Id,
                    DomainId = p.Id,
                    PositionDomainId = p.Id,
                    Warriors = RandomHelper.AddRandom(WarriorParameters.StartCount, randomNumber: RandomHelper.DependentRandom(p.Id, 0)),
                    Type = enUnitCommandType.WarSupportDefense,
                    TargetDomainId = p.Id,
                    InitiatorPersonId = p.Id,
                    Status = enCommandStatus.ReadyToMove,
                    ActionPoints = WarConstants.ActionPointsFullCount
                })
                .ToArray();

        internal static Turn GetFirstTurn()
        {
            return firstTurn;
        }

        internal static Session GetFirstGameSession()
        {
            return firstGameSession;
        }

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
