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

        public EndOfTurnService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Execute()
        {
            var currentTurn = DeactivateCurrentTurn();

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

        private Turn DeactivateCurrentTurn()
        {
            var currentTurn = _context.Turns.
                  Single(t => t.IsActive);
            currentTurn.IsActive = false;
            _context.Update(currentTurn);
            return currentTurn;
        }

        private void ExecuteGrowthAction(List<Command> currentCommands)
        {
            var growthCommands = currentCommands.Where(c => c.Type == Enums.enCommandType.Growth);
            foreach (var growthCommand in growthCommands)
            {
                var task = new GrowthAction(growthCommand);
                var success = task.Execute();
                if (success)
                    _context.Update(growthCommand);
            }
        }

        private void ExecuteIdlenessAction(List<Command> currentCommands)
        {
            var idlenessCommands = currentCommands.Where(c => c.Type == Enums.enCommandType.Idleness);
            foreach (var idlenessCommand in idlenessCommands)
            {
                var task = new IdlenessAction(idlenessCommand);
                var success = task.Execute();
                if (success)
                    _context.Update(idlenessCommand);
            }
        }

        private void ExecuteWarAction(List<Command> currentCommands)
        {
            var warCommands = currentCommands.Where(c => c.Type == Enums.enCommandType.War);
            foreach (var warCommand in warCommands)
            {
                var task = new WarAction(warCommand);
                var success = task.Execute();
                if (success)
                    _context.Update(warCommand);
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
                    _context.AddRange(task.NewCommands);
                    _context.Update(organization);
                }
            }
        }

        private Turn CreateNewTurn(Turn currentTurn)
        {
            var newTurn = new Turn
            {
                IsActive = true,
                Started = DateTime.UtcNow,
                Name = GetTurnName(currentTurn.Id + 1)
            };
            _context.Add(newTurn);
            return newTurn;
        }

        private string GetTurnName(int number)
        {
            var year = 587 + (number - 1) / 4;
            switch (number % 4)
            {
                case 1:
                    return $"{year} год - Зима";
                case 2:
                    return $"{year} год - Весна";
                case 3:
                    return $"{year} год - Лето";
                case 4:
                default:
                    return $"{year} год - Осень";
            }
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
