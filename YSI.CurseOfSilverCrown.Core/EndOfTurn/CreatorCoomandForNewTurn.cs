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
    public static class CreatorCoomandForNewTurn
    {
        private static Random _random = new Random();

        public static void CreateNewCommandsForOrganizations(ApplicationDbContext context, params Organization[] organizations)
        {
            foreach (var organization in organizations)
            {
                CreateNewCommandsForBotOrganizations(context, organization);
            }
            context.SaveChanges();
        }

        public static void CreateNewCommandsForOrganizations(ApplicationDbContext context, string initiatorId, Organization organization)
        {
            CreateNewCommandsForBotOrganizations(context, organization, initiatorId);
            context.SaveChanges();
        }

        private static void CreateNewCommandsForBotOrganizations(ApplicationDbContext context, Organization organization, string initiatorId = null)
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

        private static Command GetGrowthCommand(Organization organization, string initiatorId = null)
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
                Id = Guid.NewGuid().ToString(),
                Coffers = spendToGrowth,
                OrganizationId = organization.Id,
                Type = enCommandType.Growth,
                InitiatorOrganizationId = initiatorId ?? organization.Id,
                Status = initiatorId == null || initiatorId == organization.Id
                    ? enCommandStatus.ReadyToRun
                    : enCommandStatus.ReadyToSend
            };
        }

        private static Command GetInvestmentsCommand(Organization organization, string initiatorId = null)
        {
            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                Coffers = 0,
                OrganizationId = organization.Id,
                Type = enCommandType.Investments,
                InitiatorOrganizationId = initiatorId ?? organization.Id,
                Status = initiatorId == null || initiatorId == organization.Id
                    ? enCommandStatus.ReadyToRun
                    : enCommandStatus.ReadyToSend
            };
        }

        private static Command GetFortificationsCommand(Organization organization, string initiatorId = null)
        {
            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                Coffers = 0,
                OrganizationId = organization.Id,
                Type = enCommandType.Fortifications,
                InitiatorOrganizationId = initiatorId ?? organization.Id,
                Status = initiatorId == null || initiatorId == organization.Id
                    ? enCommandStatus.ReadyToRun
                    : enCommandStatus.ReadyToSend
            };
        }

        private static Command GetDefenceCommand(Organization organization, string initiatorId = null)
        {
            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                Warriors = organization.Warriors,
                OrganizationId = organization.Id,
                Type = enCommandType.WarSupportDefense,
                TargetOrganizationId = organization.Id,
                InitiatorOrganizationId = initiatorId ?? organization.Id,
                Status = initiatorId == null || initiatorId == organization.Id
                    ? enCommandStatus.ReadyToRun
                    : enCommandStatus.ReadyToSend
            };
        }

        private static Command GetRebelionCommand(Organization organization, string initiatorId = null)
        {
            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                Warriors = 0,
                OrganizationId = organization.Id,
                Type = enCommandType.Rebellion,
                TargetOrganizationId = organization.SuzerainId,
                InitiatorOrganizationId = initiatorId ?? organization.Id,
                Status = initiatorId == null || initiatorId == organization.Id
                    ? enCommandStatus.ReadyToRun
                    : enCommandStatus.ReadyToSend
            };
        }

        private static Command GetCollectTaxCommand(Organization organization, string initiatorId = null)
        {
            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                OrganizationId = organization.Id,
                Warriors = 0,
                Type = enCommandType.CollectTax,
                InitiatorOrganizationId = initiatorId ?? organization.Id,
                Status = initiatorId == null || initiatorId == organization.Id
                    ? enCommandStatus.ReadyToRun
                    : enCommandStatus.ReadyToSend
            };
        }

        private static Command GetIdlenessCommand(Organization organization, string initiatorId = null)
        {
            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                Coffers = RandomHelper.AddRandom(Constants.MinIdleness, roundRequest: -1),
                OrganizationId = organization.Id,
                Type = enCommandType.Idleness,
                InitiatorOrganizationId = initiatorId ?? organization.Id,
                Status = initiatorId == null || initiatorId == organization.Id
                    ? enCommandStatus.ReadyToRun
                    : enCommandStatus.ReadyToSend
            };
        }
    }
}
