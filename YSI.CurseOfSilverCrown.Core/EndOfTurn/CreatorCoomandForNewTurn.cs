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
    public class CreatorCoomandForNewTurn
    {
        private Random _random = new Random();

        public void CreateNewCommandsForOrganizations(ApplicationDbContext context, List<Organization> organizations)
        {
            foreach (var organization in organizations)
            {
                CreateNewCommandsForBotOrganizations(context, organization);
            }
            context.SaveChanges();
        }

        private void CreateNewCommandsForBotOrganizations(ApplicationDbContext context, Organization organization)
        {
            var tax = GetCollectTaxCommand(organization);

            var growth = GetGrowthCommand(organization, out var needMoney, out var nextTurnWarriors);

            var investments = GetInvestmentsCommand(organization);

            var fortifications = GetFortificationsCommand(organization);

            var defence = GetDefenceCommand(organization);

            var idleness = GetIdlenessCommand(organization, needMoney, nextTurnWarriors);

            context.AddRange(tax, growth, investments, fortifications, idleness, defence);
        }

        private Command GetGrowthCommand(Organization organization, out int needMoney, out int nextTurnWarriors)
        {
            var wantWarriors = Math.Max(0, WarriorParameters.StartCount - organization.Warriors);
            var wantWarriorsRandom = wantWarriors > 0
                ? Math.Max(0, wantWarriors + _random.Next(20))
                : 0;
            needMoney = wantWarriorsRandom * (WarriorParameters.Maintenance + WarriorParameters.Price);
            if (needMoney > organization.Coffers)
            {
                wantWarriorsRandom = organization.Coffers / (WarriorParameters.Maintenance + WarriorParameters.Price);
                needMoney = wantWarriorsRandom * (WarriorParameters.Maintenance + WarriorParameters.Price);
            }
            var spendToGrowth = wantWarriorsRandom * WarriorParameters.Price;

            nextTurnWarriors = Math.Max(0, wantWarriors - wantWarriorsRandom);

            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                Coffers = spendToGrowth,
                OrganizationId = organization.Id,
                Type = enCommandType.Growth
            };
        }

        private Command GetInvestmentsCommand(Organization organization)
        {
            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                Coffers = 0,
                OrganizationId = organization.Id,
                Type = enCommandType.Investments
            };
        }

        private object GetFortificationsCommand(Organization organization)
        {
            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                Coffers = 0,
                OrganizationId = organization.Id,
                Type = enCommandType.Fortifications
            };
        }

        private Command GetDefenceCommand(Organization organization)
        {
            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                Warriors = organization.Warriors,
                OrganizationId = organization.Id,
                Type = enCommandType.WarSupportDefense,
                TargetOrganizationId = organization.Id
            };
        }

        private Command GetCollectTaxCommand(Organization organization)
        {
            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                OrganizationId = organization.Id,
                Warriors = 0,
                Type = enCommandType.CollectTax
            };
        }

        private Command GetIdlenessCommand(Organization organization, int needMoney, int nextTurnWarriors)
        {
            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                Coffers = RandomHelper.AddRandom(Constants.MinIdleness, roundRequest: -1),
                OrganizationId = organization.Id,
                Type = enCommandType.Idleness
            };
        }
    }
}
