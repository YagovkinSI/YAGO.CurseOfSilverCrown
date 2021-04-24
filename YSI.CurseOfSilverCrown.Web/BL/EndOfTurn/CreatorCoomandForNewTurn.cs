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
                Warriors = organization.Warriors - 30,
                OrganizationId = organization.Id,
                Type = Enums.enCommandType.WarSupportDefense
            };
        }

        private Command GetCollectTaxCommand(Organization organization)
        {
            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                OrganizationId = organization.Id,
                Warriors = 30,
                Type = Enums.enCommandType.CollectTax
            };
        }

        private Command GetIdlenessCommand(Organization organization, int needMoney, int nextTurnWarriors)
        {
            if (organization.User != null && organization.User.LastActivityTime > DateTime.UtcNow - Constants.CorruptionStartTime)
            {
                return new Command
                {
                    Id = Guid.NewGuid().ToString(),
                    Coffers = Constants.AddRandom10(Constants.MinIdleness, _random.NextDouble()),
                    OrganizationId = organization.Id,
                    Type = Enums.enCommandType.Idleness
                };
            }

            var spareMoney = organization.Coffers
                + Constants.MinTax
                + Constants.GetAdditionalTax(organization.Warriors - Constants.MinTaxAuthorities, 0.5)
                + (organization.Vassals.Count * Constants.VassalTax)
                - Constants.MinIdleness
                - needMoney
                - organization.Warriors * Constants.MaintenanceWarrioir
                - (organization.Suzerain != null ? Constants.VassalTax : 0);
            if (nextTurnWarriors > 0)
                spareMoney -= (nextTurnWarriors + 20) * (Constants.OutfitWarrioir + Constants.MaintenanceWarrioir);
            if (spareMoney > Constants.MaxIdleness - Constants.MinIdleness)
                spareMoney = Constants.MaxIdleness - Constants.MinIdleness;
            var random = organization.Warriors < Constants.BaseCountWarriors
                ? _random.NextDouble() / 2.0
                : _random.NextDouble() / 2.0 + 0.5;
            spareMoney = Constants.AddRandom10(spareMoney, random);
            if (spareMoney < 0)
                spareMoney = 0;
            return new Command
            {
                Id = Guid.NewGuid().ToString(),
                Coffers = Constants.MinIdleness + spareMoney,
                OrganizationId = organization.Id,
                Type = Enums.enCommandType.Idleness
            };
        }
    }
}
