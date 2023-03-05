using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Commands;
using YSI.CurseOfSilverCrown.Core.Database.Domains;
using YSI.CurseOfSilverCrown.Core.Database.Units;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Commands
{
    public static class CommandCreateForNewTurnHelper
    {
        public static void CreateNewCommandsForOrganizations(ApplicationDbContext context)
        {
            var domains = context.Domains.ToArray();
            foreach (var organization in domains)
            {
                CreateNewCommandsForBotOrganizations(context, organization, organization.PersonId);
            }
            context.SaveChanges();
        }

        public static void CreateNewCommandsForOrganizations(ApplicationDbContext context, int initiatorId, Domain domain)
        {
            CreateNewCommandsForBotOrganizations(context, domain, initiatorId);
            context.SaveChanges();
        }

        private static void CreateNewCommandsForBotOrganizations(ApplicationDbContext context, Domain domain, int initiatorId)
        {
            var growth = GetGrowthCommand(context, domain, initiatorId);
            var investments = GetInvestmentsCommand(domain, initiatorId);
            var fortifications = GetFortificationsCommand(domain, initiatorId);
            context.AddRange(growth, investments, fortifications);

            if (initiatorId != domain.PersonId)
            {
                var domainUnits = context.Units
                    .Where(d => d.DomainId == domain.Id && d.InitiatorPersonId == domain.PersonId);
                var newUnits = new List<Unit>();
                foreach (var unit in domainUnits)
                {
                    var newUnit = new Unit
                    {
                        DomainId = unit.DomainId,
                        PositionDomainId = unit.PositionDomainId,
                        Warriors = unit.Warriors,
                        Type = UnitCommandType.WarSupportDefense,
                        TargetDomainId = unit.PositionDomainId,
                        InitiatorPersonId = initiatorId,
                        Status = CommandStatus.ReadyToMove,
                        ActionPoints = WarConstants.ActionPointsFullCount
                    };
                    newUnits.Add(newUnit);
                }
                context.AddRange(newUnits);
            }
        }

        private static Command GetGrowthCommand(ApplicationDbContext context, Domain domain, int? initiatorId = null)
        {
            return new Command
            {
                Coffers = 0,
                DomainId = domain.Id,
                Type = CommandType.Growth,
                InitiatorPersonId = initiatorId ?? domain.PersonId,
                Status = CommandStatus.ReadyToMove
            };
        }

        private static Command GetInvestmentsCommand(Domain organization, int? initiatorId = null)
        {
            return new Command
            {
                Coffers = 0,
                DomainId = organization.Id,
                Type = CommandType.Investments,
                InitiatorPersonId = initiatorId ?? organization.PersonId,
                Status = CommandStatus.ReadyToMove
            };
        }

        private static Command GetFortificationsCommand(Domain domain, int? initiatorId = null)
        {
            return new Command
            {
                Coffers = 0,
                DomainId = domain.Id,
                Type = CommandType.Fortifications,
                InitiatorPersonId = initiatorId ?? domain.PersonId,
                Status = CommandStatus.ReadyToMove
            };
        }
    }
}
