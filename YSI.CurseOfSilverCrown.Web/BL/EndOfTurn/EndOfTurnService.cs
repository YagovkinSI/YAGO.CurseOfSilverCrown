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
                .Where(c => c.TurnId == currentTurn.Id).ToList();
            var organizations = _context.Organizations
                .Include(o => o.Suzerain)
                .ToList();

            ExecuteWarAction(currentCommands);
            ExecuteGrowthAction(currentCommands);
            ExecuteIdlenessAction(currentCommands);
            ExecuteVassalTaxAction(currentTurn, organizations);

            var newTurn = CreateNewTurn(currentTurn);
            CreateNewCommandsForOrganizations(newTurn, organizations);

            var changed = await _context.SaveChangesAsync();

            return changed > 0;
        }

        private void DeactivateCurrentTurn(Turn currentTurn)
        {            
            currentTurn.IsActive = false;
            _context.Update(currentTurn);
        }

        private void ExecuteGrowthAction(List<Command> currentCommands)
        {
            var growthCommands = currentCommands.Where(c => c.Type == Enums.enCommandType.Growth);
            foreach (var command in growthCommands)
            {
                var task = new GrowthAction(command);
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

        private void ExecuteIdlenessAction(List<Command> currentCommands)
        {
            var idlenessCommands = currentCommands.Where(c => c.Type == Enums.enCommandType.Idleness);
            foreach (var command in idlenessCommands)
            {
                var task = new IdlenessAction(command);
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

        private void ExecuteWarAction(List<Command> currentCommands)
        {
            var warCommands = currentCommands.Where(c => c.Type == Enums.enCommandType.War);
            foreach (var command in warCommands)
            {
                var task = new WarAction(command);
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

        private Turn CreateNewTurn(Turn currentTurn)
        {
            var newTurn = new Turn
            {
                IsActive = true,
                Started = DateTime.UtcNow
            };
            _context.Add(newTurn);
            return newTurn;
        }

        private void CreateNewCommandsForOrganizations(Turn newTurn, List<Organization> organizations)
        {
            foreach (var organization in organizations)
            {
                var command = new Command
                {
                    Id = Guid.NewGuid().ToString(),
                    OrganizationId = organization.Id,
                    Turn = newTurn,
                    Type = Enums.enCommandType.Idleness
                };
                _context.Add(command);
            }
        }
    }
}
