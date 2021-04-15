using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions;
using YSI.CurseOfSilverCrown.Web.Data;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn
{
    public class EndOfTurnService
    {
        private Random _random = new Random();

        private ApplicationDbContext _context;

        private int number; 

        public EndOfTurnService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Execute()
        {
            var currentTurn = _context.Turns.
                  Single(t => t.IsActive);

            number = 1;

            DeactivateCurrentTurn(currentTurn);

            var currentCommands = _context.Commands
                .Include(c => c.Organization)
                .Include(c => c.Target)
                .ToList();
            var organizations = _context.Organizations
                .Include(o => o.Suzerain)
                .Include(o => o.Vassals)
                .ToList();

            ExecuteWarAction(currentTurn, currentCommands);
            ExecuteGrowthAction(currentTurn, currentCommands);
            ExecuteTaxAction(currentTurn, currentCommands);
            ExecuteVassalTaxAction(currentTurn, organizations);
            ExecuteIdlenessAction(currentTurn, currentCommands);
            ExecuteMaintenanceAction(currentTurn, organizations);
            ExecuteMutinyAction(currentTurn, organizations);

            var newTurn = CreateNewTurn();
            CreateNewCommandsForOrganizations(organizations);

            var changed = await _context.SaveChangesAsync();

            return changed > 0;
        }

        private void DeactivateCurrentTurn(Turn currentTurn)
        {            
            currentTurn.IsActive = false;
            _context.Update(currentTurn);
        }

        private void ExecuteGrowthAction(Turn currentTurn, List<Command> currentCommands)
        {
            var growthCommands = currentCommands.Where(c => c.Type == Enums.enCommandType.Growth);
            foreach (var command in growthCommands)
            {
                if (command.Coffers < Constants.OutfitWarrioir)
                {
                    _context.Remove(command);
                    continue;
                }
                var task = new GrowthAction(command, currentTurn);                
                var success = task.Execute();
                if (success)
                {
                    task.EventStory.Id = number;
                    number++;
                    _context.Add(task.EventStory);
                    _context.AddRange(task.OrganizationEventStories);
                    _context.Remove(command);
                }
            }
        }

        private void ExecuteIdlenessAction(Turn currentTurn, List<Command> currentCommands)
        {
            var idlenessCommands = currentCommands.Where(c => c.Type == Enums.enCommandType.Idleness);
            foreach (var command in idlenessCommands)
            {
                if (command.Coffers <= 0)
                {
                    _context.Remove(command);
                    continue;
                }
                var task = new IdlenessAction(command, currentTurn);
                var success = task.Execute();
                if (success)
                {
                    task.EventStory.Id = number;
                    number++;
                    _context.Add(task.EventStory);
                    _context.AddRange(task.OrganizationEventStories);
                    _context.Remove(command);
                }
            }
        }

        private void ExecuteWarAction(Turn currentTurn, List<Command> currentCommands)
        {
            var warCommands = currentCommands.Where(c => c.Type == Enums.enCommandType.War);
            foreach (var command in warCommands)
            {
                if (command.Warriors <= 0)
                {
                    _context.Remove(command);
                    continue;
                }
                var task = new WarAction(command, currentTurn);
                var success = task.Execute();
                if (success)
                {
                    task.EventStory.Id = number;
                    number++;
                    _context.Add(task.EventStory);
                    _context.AddRange(task.OrganizationEventStories);
                    _context.Remove(command);
                }
            }
        }

        private void ExecuteTaxAction(Turn currentTurn, List<Command> currentCommands)
        {
            var warCommands = currentCommands.Where(c => c.Type == Enums.enCommandType.CollectTax);
            foreach (var command in warCommands)
            {
                if (command.Warriors <= 0)
                {
                    _context.Remove(command);
                    continue;
                }
                var task = new TaxAction(command, currentTurn);
                var success = task.Execute();
                if (success)
                {
                    task.EventStory.Id = number;
                    number++;
                    _context.Add(task.EventStory);
                    _context.AddRange(task.OrganizationEventStories);
                    _context.Remove(command);
                }
            }
        }

        private void ExecuteVassalTaxAction(Turn currentTurn, List<Organization> organizations)
        {
            var vassals = organizations.Where(c => c.Suzerain != null);
            foreach (var organization in vassals)
            {
                var task = new VassalAction(organization, currentTurn);
                var success = task.Execute();
                if (success)
                {
                    task.EventStory.Id = number;
                    number++;
                    _context.Add(task.EventStory);
                    _context.AddRange(task.OrganizationEventStories);
                }
            }
        }

        private void ExecuteMaintenanceAction(Turn currentTurn, List<Organization> organizations)
        {
            foreach (var organization in organizations)
            {
                var task = new MaintenanceAction(organization, currentTurn);
                var success = task.Execute();
                if (success)
                {
                    task.EventStory.Id = number;
                    number++;
                    _context.Add(task.EventStory);
                    _context.AddRange(task.OrganizationEventStories);
                }
            }
        }

        private void ExecuteMutinyAction(Turn currentTurn, List<Organization> organizations)
        {
            var bankrupts = organizations.Where(c => c.Warriors < 40);
            foreach (var organization in bankrupts)
            {
                var task = new MutinyAction(organization, currentTurn);
                var success = task.Execute();
                if (success)
                {
                    task.EventStory.Id = number;
                    number++;
                    _context.Add(task.EventStory);
                    _context.AddRange(task.OrganizationEventStories);
                }
            }
        }

        private Turn CreateNewTurn()
        {
            var newTurn = new Turn
            {
                IsActive = true,
                Started = DateTime.UtcNow
            };
            _context.Add(newTurn);
            return newTurn;
        }

        private void CreateNewCommandsForOrganizations(List<Organization> organizations)
        {
            foreach (var organization in organizations)
            {
                var tax = new Command
                {
                    Id = Guid.NewGuid().ToString(),
                    OrganizationId = organization.Id,
                    Warriors = organization.Warriors,
                    Type = Enums.enCommandType.CollectTax
                };

                var wantWarriors = Math.Max(0, Constants.BaseCountWarriors - organization.Warriors);
                var wantWarriorsRandom = wantWarriors > 0
                    ? Math.Max(0, wantWarriors + _random.Next(20))
                    : 0;
                var needMoney = wantWarriorsRandom * (Constants.MaintenanceWarrioir + Constants.OutfitWarrioir);
                if (needMoney > organization.Coffers)
                {
                    wantWarriorsRandom = organization.Coffers / (Constants.MaintenanceWarrioir + Constants.OutfitWarrioir);
                    needMoney = wantWarriorsRandom * (Constants.MaintenanceWarrioir + Constants.OutfitWarrioir);
                }
                var spendToGrowth = wantWarriorsRandom * Constants.OutfitWarrioir;
                var growth = new Command
                {
                    Id = Guid.NewGuid().ToString(),
                    Coffers = spendToGrowth,
                    OrganizationId = organization.Id,
                    Type = Enums.enCommandType.Growth
                };

                var spareMoney = organization.Coffers
                    + Constants.MinTax
                    + Constants.GetAdditionalTax(organization.Warriors - Constants.MinTaxAuthorities, 0.5)
                    + (organization.Vassals.Count * Constants.VassalTax)
                    - Constants.MinIdleness
                    - needMoney
                    - organization.Warriors * Constants.MaintenanceWarrioir
                    - (organization.Suzerain != null ? Constants.VassalTax : 0);
                if (wantWarriors > 0 && wantWarriorsRandom < wantWarriors)
                    spareMoney -= (wantWarriors + 20 - wantWarriorsRandom) * (Constants.OutfitWarrioir + Constants.MaintenanceWarrioir);
                if (spareMoney > Constants.MaxIdleness - Constants.MinIdleness)
                    spareMoney = Constants.MaxIdleness - Constants.MinIdleness;
                var random = wantWarriors > 0
                    ? _random.NextDouble() / 2.0
                    : _random.NextDouble() / 2.0 + 0.5;
                spareMoney = Constants.AddRandom10(spareMoney, random);
                if (spareMoney < 0)
                    spareMoney = 0;
                var idleness = new Command
                {
                    Id = Guid.NewGuid().ToString(),
                    Coffers = Constants.MinIdleness + spareMoney,
                    OrganizationId = organization.Id,
                    Type = Enums.enCommandType.Idleness
                };

                _context.AddRange(tax, growth, idleness);
            }
        }
    }
}
