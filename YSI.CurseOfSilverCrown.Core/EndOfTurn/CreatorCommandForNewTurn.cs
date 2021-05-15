using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.EndOfTurn
{
    public static class CreatorCommandForNewTurn
    {
        private static Random _random = new Random();

        public static void CreateNewCommandsForOrganizations(ApplicationDbContext context, params Domain[] organizations)
        {
            foreach (var organization in organizations)
            {
                CreateNewCommandsForBotOrganizations(context, organization);
            }
            context.SaveChanges();
        }

        public static void CreateNewCommandsForOrganizations(ApplicationDbContext context, int? initiatorId, Domain organization)
        {
            CreateNewCommandsForBotOrganizations(context, organization, initiatorId);
            context.SaveChanges();
        }

        private static void CreateNewCommandsForBotOrganizations(ApplicationDbContext context, Domain organization, int? initiatorId = null)
        {
            var tax = GetCollectTaxCommand(organization, initiatorId);

            var growth = GetGrowthCommand(organization, initiatorId);

            var investments = GetInvestmentsCommand(organization, initiatorId);

            var fortifications = GetFortificationsCommand(organization, initiatorId);

            var defence = GetDefenceCommand(organization, initiatorId);

            var rebelion = GetRebelionCommand(organization, initiatorId);

            var idleness = GetIdlenessCommand(organization, initiatorId);

            context.AddRange(tax, growth, investments, fortifications, rebelion, idleness, defence);
        }

        private static Command GetGrowthCommand(Domain organization, int? initiatorId = null)
        {
            var wantWarriors = Math.Max(0, WarriorParameters.StartCount - organization.Warriors);
            var wantWarriorsRandom = wantWarriors > 0
                ? Math.Max(0, wantWarriors + _random.Next(20))
                : 0;
            var needMoney = wantWarriorsRandom * (WarriorParameters.Maintenance + WarriorParameters.Price);
            if (needMoney > organization.Coffers)
            {
                wantWarriorsRandom = organization.Coffers / (WarriorParameters.Maintenance + WarriorParameters.Price);
            }
            var spendToGrowth = wantWarriorsRandom * WarriorParameters.Price;

            return new Command
            {
                Coffers = spendToGrowth,
                DomainId = organization.Id,
                Type = enCommandType.Growth,
                InitiatorDomainId = initiatorId ?? organization.Id,
                Status = initiatorId == null || initiatorId == organization.Id
                    ? enCommandStatus.ReadyToRun
                    : enCommandStatus.ReadyToSend
            };
        }

        private static Command GetInvestmentsCommand(Domain organization, int? initiatorId = null)
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

        private static Command GetFortificationsCommand(Domain organization, int? initiatorId = null)
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

        private static Unit GetDefenceCommand(Domain organization, int? initiatorId = null)
        {
            return new Unit
            {
                DomainId = organization.Id,
                Warriors = organization.Warriors,
                Type = enArmyCommandType.WarSupportDefense,
                TargetDomainId = organization.Id,
                InitiatorDomainId = initiatorId ?? organization.Id,
                Status = initiatorId == null || initiatorId == organization.Id
                    ? enCommandStatus.ReadyToRun
                    : enCommandStatus.ReadyToSend
            };
        }

        private static Unit GetRebelionCommand(Domain organization, int? initiatorId = null)
        {
            return new Unit
            {
                Warriors = 0,
                DomainId = organization.Id,
                Type = enArmyCommandType.Rebellion,
                TargetDomainId = organization.SuzerainId,
                InitiatorDomainId = initiatorId ?? organization.Id,
                Status = initiatorId == null || initiatorId == organization.Id
                    ? enCommandStatus.ReadyToRun
                    : enCommandStatus.ReadyToSend
            };
        }

        private static Unit GetCollectTaxCommand(Domain organization, int? initiatorId = null)
        {
            return new Unit
            {
                DomainId = organization.Id,
                Warriors = 0,
                Type = enArmyCommandType.CollectTax,
                InitiatorDomainId = initiatorId ?? organization.Id,
                Status = initiatorId == null || initiatorId == organization.Id
                    ? enCommandStatus.ReadyToRun
                    : enCommandStatus.ReadyToSend
            };
        }

        private static Command GetIdlenessCommand(Domain organization, int? initiatorId = null)
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
