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
                var domainUnits = domain.Units
                        .Where(u => u.DomainId == domain.Id);
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
                        Status = enCommandStatus.ReadyToSend
                    };
                    context.Add(newUnit);
                }
            }
        }

        private static Command GetGrowthCommand(ApplicationDbContext context, DomainMin domain, int? initiatorId = null)
        {
            var warriors = DomainHelper.GetWarriorCount(context, domain.Id);
            var wantWarriors = Math.Max(0, WarriorParameters.StartCount - warriors);
            var wantWarriorsRandom = wantWarriors > 0
                ? Math.Max(0, wantWarriors + _random.Next(20))
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
                Status = initiatorId == null || initiatorId == domain.Id
                    ? enCommandStatus.ReadyToRun
                    : enCommandStatus.ReadyToSend
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
                Status = initiatorId == null || initiatorId == organization.Id
                    ? enCommandStatus.ReadyToRun
                    : enCommandStatus.ReadyToSend
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
                Status = initiatorId == null || initiatorId == organization.Id
                    ? enCommandStatus.ReadyToRun
                    : enCommandStatus.ReadyToSend
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
                Status = initiatorId == null || initiatorId == organization.Id
                    ? enCommandStatus.ReadyToRun
                    : enCommandStatus.ReadyToSend
            };
        }
    }
}
