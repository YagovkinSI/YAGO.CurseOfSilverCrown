using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.BL.Models.Main;
using YSI.CurseOfSilverCrown.Core.BL.Models.Min;
using YSI.CurseOfSilverCrown.Core.BL.Models;

namespace YSI.CurseOfSilverCrown.EndOfTurn
{
    public static class CreatorCommandForNewTurn
    {
        private static Random _random = new Random();

        public static void CreateNewCommandsForOrganizations(ApplicationDbContext context, params DomainMain[] domains)
        {
            foreach (var organization in domains)
            {
                CreateNewCommandsForBotOrganizations(context, organization, organization.Id);
            }
            context.SaveChanges();
        }

        public static void CreateNewCommandsForOrganizations(ApplicationDbContext context, int initiatorId, DomainMain domain)
        {
            CreateNewCommandsForBotOrganizations(context, domain, initiatorId);
            context.SaveChanges();
        }

        private static void CreateNewCommandsForBotOrganizations(ApplicationDbContext context, DomainMain domain, int initiatorId)
        {
            var growth = GetGrowthCommand(context, domain, initiatorId);
            var investments = GetInvestmentsCommand(domain, initiatorId);
            var fortifications = GetFortificationsCommand(domain, initiatorId);
            var idleness = GetIdlenessCommand(domain, initiatorId);
            context.AddRange(growth, investments, fortifications, idleness);                     

            if (initiatorId != domain.Id)
            {
                var domainUnits = context.GetUnitsMainAsync(domain.Id, domain.Id).Result;
                var newUnits = new List<Unit>();
                foreach (var unit in domainUnits)
                {
                    var newUnit = new Unit
                    {
                        DomainId = unit.DomainId,
                        PositionDomainId = unit.PositionDomainId,
                        Warriors = unit.Warriors,
                        Type = enArmyCommandType.WarSupportDefense,
                        TargetDomainId = unit.PositionDomainId,
                        InitiatorDomainId = initiatorId,
                        Status = enCommandStatus.ReadyToMove,
                        ActionPoints = WarConstants.ActionPointsFullCount
                    };
                    newUnits.Add(newUnit);
                }
                context.AddRange(newUnits);
            }
        }

        private static Command GetGrowthCommand(ApplicationDbContext context, DomainMain domain, int? initiatorId = null)
        {
            var warriors = domain.Warriors;
            var wantWarriors = Math.Max(0, WarriorParameters.StartCount * 1.1 - warriors);
            var wantWarriorsRandom = wantWarriors > 0
                ? (int)Math.Max(0, wantWarriors - _random.Next(20))
                : 0;
            var needMoney = wantWarriorsRandom * (WarriorParameters.Maintenance + WarriorParameters.Price);
            if (needMoney > domain.Coffers)
            {
                wantWarriorsRandom = domain.Coffers / (WarriorParameters.Maintenance + WarriorParameters.Price);
            }
            var spendToGrowth = wantWarriorsRandom * WarriorParameters.Price;

            return new Command
            {
                Coffers = spendToGrowth,
                DomainId = domain.Id,
                Type = enCommandType.Growth,
                InitiatorDomainId = initiatorId ?? domain.Id,
                Status = enCommandStatus.ReadyToMove
            };
        }

        private static Command GetInvestmentsCommand(DomainMin organization, int? initiatorId = null)
        {
            return new Command
            {
                Coffers = 0,
                DomainId = organization.Id,
                Type = enCommandType.Investments,
                InitiatorDomainId = initiatorId ?? organization.Id,
                Status = enCommandStatus.ReadyToMove
            };
        }

        private static Command GetFortificationsCommand(DomainMin organization, int? initiatorId = null)
        {
            return new Command
            {
                Coffers = 0,
                DomainId = organization.Id,
                Type = enCommandType.Fortifications,
                InitiatorDomainId = initiatorId ?? organization.Id,
                Status = enCommandStatus.ReadyToMove
            };
        }

        private static Command GetIdlenessCommand(DomainMin organization, int? initiatorId = null)
        {
            return new Command
            {
                Coffers = RandomHelper.AddRandom(Constants.MinIdleness, roundRequest: -1),
                DomainId = organization.Id,
                Type = enCommandType.Idleness,
                InitiatorDomainId = initiatorId ?? organization.Id,
                Status = enCommandStatus.ReadyToMove
            };
        }
    }
}
