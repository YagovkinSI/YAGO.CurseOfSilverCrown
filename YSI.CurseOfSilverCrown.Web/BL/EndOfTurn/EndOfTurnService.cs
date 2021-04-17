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

        private CreatorCoomandForNewTurn CreatorCoomandForNewTurn { get; set; }

        public EndOfTurnService(ApplicationDbContext context)
        {
            _context = context;

            CreatorCoomandForNewTurn = new CreatorCoomandForNewTurn();
        }

        internal void CreateCommands()
        {
            var organizations = _context.Organizations
                .Include(o => o.User)
                .Include(o => o.Suzerain)
                .Include(o => o.Vassals)
                .ToList();
            CreatorCoomandForNewTurn.CreateNewCommandsForOrganizations(_context, organizations);
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
                .Include("Target.Commands")
                .ToList();
            var organizations = _context.Organizations
                .Include(o => o.User)
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
            CreatorCoomandForNewTurn.CreateNewCommandsForOrganizations(_context, organizations);

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
    }
}
