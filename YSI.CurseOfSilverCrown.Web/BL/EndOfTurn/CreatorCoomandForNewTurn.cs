using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Data;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn
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

            var defence = GetDefenceCommand(organization);

            var idleness = GetIdlenessCommand(organization, needMoney, nextTurnWarriors);

            context.AddRange(tax, growth, investments, idleness, defence);
        }

        private Command GetGrowthCommand(Organization organization, out int needMoney, out int nextTurnWarriors)
        {
            var wantWarriors = Math.Max(0, Constants.BaseCountWarriors - organization.Warriors);
            var wantWarriorsRandom = wantWarriors > 0
                ? Math.Max(0, wantWarriors + _random.Next(20))
                : 0;
            needMoney = wantWarriorsRandom * (Constants.MaintenanceWarrioir + Constants.OutfitWarrioir);
            if (needMoney > organization.Coffers)
            {
                wantWarriorsRandom = organization.Coffers / (Constants.MaintenanceWarrioir + Constants.OutfitWarrioir);
                needMoney = wantWarriorsRandom * (Constants.MaintenanceWarrioir + Constants.OutfitWarrioir);
            }
            var spendToGrowth = wantWarriorsRandom * Constants.OutfitWarrioir;

            nextTurnWarriors = Math.Max(0, wantWarriors - wantWarriorsRandom);

            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                Coffers = spendToGrowth,
                OrganizationId = organization.Id,
                Type = Enums.enCommandType.Growth
            };
        }

        private Command GetInvestmentsCommand(Organization organization)
        {
            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                Coffers = 0,
                OrganizationId = organization.Id,
                Type = Enums.enCommandType.Investments
            };
        }

        private Command GetDefenceCommand(Organization organization)
        {
            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                Warriors = organization.Warriors - Constants.MinTaxAuthorities,
                OrganizationId = organization.Id,
                Type = Enums.enCommandType.WarSupportDefense,
                TargetOrganizationId = organization.Id
            };
        }

        private Command GetCollectTaxCommand(Organization organization)
        {
            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                OrganizationId = organization.Id,
                Warriors = Constants.MinTaxAuthorities,
                Type = Enums.enCommandType.CollectTax
            };
        }

        private Command GetIdlenessCommand(Organization organization, int needMoney, int nextTurnWarriors)
        {
            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                Coffers = Constants.AddRandom10(Constants.MinIdleness, _random.NextDouble()),
                OrganizationId = organization.Id,
                Type = Enums.enCommandType.Idleness
            };
        }
    }
}
