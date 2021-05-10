using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Actions;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.EndOfTurn
{
    public class EndOfTurnService
    {
        private ApplicationDbContext _context;

        private int number; 

        public EndOfTurnService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateCommands()
        {
            var organizations = _context.Organizations
                .Include(o => o.User)
                .Include(o => o.Suzerain)
                .Include(o => o.Vassals)
                .ToArray();
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
                .ToList();
            var organizations = _context.Organizations
                .Include(o => o.User)
                .Include(o => o.Suzerain)
                .Include(o => o.Vassals)
                .ToArray();

            CheckSuzerainCommand(organizations);

            ExecuteRebelionAction(currentTurn, currentCommands);
            ExecuteVassalTransferAction(currentTurn, currentCommands);
            ExecuteWarAction(currentTurn, currentCommands);
            ExecuteGoldTransferAction(currentTurn, currentCommands);
            ExecuteGrowthAction(currentTurn, currentCommands);
            ExecuteInvestmentsAction(currentTurn, currentCommands);
            ExecuteFortificationsAction(currentTurn, currentCommands);
            ExecuteTaxAction(currentTurn, currentCommands);
            //ExecuteVassalTaxAction(currentTurn, organizations);
            ExecuteIdlenessAction(currentTurn, currentCommands);
            ExecuteFortificationsMaintenanceAction(currentTurn, organizations);
            ExecuteMaintenanceAction(currentTurn, organizations);
            ExecuteCorruptionAction(currentTurn, organizations);
            ExecuteMutinyAction(currentTurn, organizations);


            _context.RemoveRange(_context.Commands);

            var newTurn = CreateNewTurn();
            CreatorCoomandForNewTurn.CreateNewCommandsForOrganizations(_context, organizations);

            var changed = await _context.SaveChangesAsync();

            return changed > 0;
        }

        private void CheckSuzerainCommand(Organization[] organizations)
        {
            foreach (var organization in organizations)
            {
                if (organization.User != null && organization.User.LastActivityTime > DateTime.UtcNow - new TimeSpan(24, 0, 0))
                    continue;
                if (!organization.Commands.Any(c => c.InitiatorOrganizationId == organization.SuzerainId))
                    continue;

                var botCommands = organization.Commands.Where(c => c.InitiatorOrganizationId == organization.Id);
                _context.RemoveRange(botCommands);

                var suzerainCommands = organization.Commands.Where(c => c.InitiatorOrganizationId == organization.SuzerainId);
                foreach (var suzerainCommand in suzerainCommands)
                    suzerainCommand.Status = enCommandStatus.ReadyToRun;  
            }
        }

        private void DeactivateCurrentTurn(Turn currentTurn)
        {            
            currentTurn.IsActive = false;
            _context.Update(currentTurn);
        }

        private void ExecuteGoldTransferAction(Turn currentTurn, List<Command> currentCommands)
        {
            var commands = currentCommands.Where(c => c.Type == enCommandType.GoldTransfer && c.Status == enCommandStatus.ReadyToRun);
            foreach (var command in commands)
            {
                var task = new GoldTransferAction(_context, currentTurn, command);
                number = task.ExecuteAction(number, true);
            }
        }

        private void ExecuteGrowthAction(Turn currentTurn, List<Command> currentCommands)
        {
            var commands = currentCommands.Where(c => c.Type == enCommandType.Growth && c.Status == enCommandStatus.ReadyToRun);
            foreach (var command in commands)
            {
                if (command.Coffers < WarriorParameters.Price)
                {
                    _context.Remove(command);
                    continue;
                }
                var task = new GrowthAction(_context, currentTurn, command);
                number = task.ExecuteAction(number, true);
            }
        }

        private void ExecuteInvestmentsAction(Turn currentTurn, List<Command> currentCommands)
        {
            var commands = currentCommands.Where(c => c.Type == enCommandType.Investments && c.Status == enCommandStatus.ReadyToRun);
            foreach (var command in commands)
            {
                if (command.Coffers <= 0)
                {
                    _context.Remove(command);
                    continue;
                }
                var task = new InvestmentsAction(_context, currentTurn, command);
                number = task.ExecuteAction(number, true);
            }
        }

        private void ExecuteFortificationsAction(Turn currentTurn, List<Command> currentCommands)
        {
            var commands = currentCommands.Where(c => c.Type == enCommandType.Fortifications && c.Status == enCommandStatus.ReadyToRun);
            foreach (var command in commands)
            {
                if (command.Coffers <= 0)
                {
                    _context.Remove(command);
                    continue;
                }
                var task = new FortificationsAction(_context, currentTurn, command);
                number = task.ExecuteAction(number, true);
            }
        }

        private void ExecuteIdlenessAction(Turn currentTurn, List<Command> currentCommands)
        {
            var idlenessCommands = currentCommands.Where(c => c.Type == enCommandType.Idleness && c.Status == enCommandStatus.ReadyToRun);
            foreach (var command in idlenessCommands)
            {
                if (command.Coffers <= 0)
                {
                    _context.Remove(command);
                    continue;
                }
                var task = new IdlenessAction(_context, currentTurn, command);
                number = task.ExecuteAction(number, true);
            }
        }

        private void ExecuteRebelionAction(Turn currentTurn, List<Command> currentCommands)
        {
            var commands = currentCommands.Where(c => c.Type == enCommandType.Rebellion && c.Status == enCommandStatus.ReadyToRun);
            foreach (var command in commands)
            {
                if (command.Warriors <= 0 || command.TargetOrganizationId != command.Organization.SuzerainId)
                {
                    _context.Remove(command);
                    continue;
                }
                var task = new RebelionAction(_context, currentTurn, command);
                number = task.ExecuteAction(number, true);
            }
        }

        private void ExecuteWarAction(Turn currentTurn, List<Command> currentCommands)
        {
            var warCommands = currentCommands
                .Where(c => c.Type == enCommandType.War && c.Status == enCommandStatus.ReadyToRun)
                .OrderBy(c => c.Warriors);
            foreach (var command in warCommands)
            {
                if (command.Warriors <= 0)
                {
                    _context.Remove(command);
                    continue;
                }
                var task = new WarAction(_context, currentTurn, command);
                number = task.ExecuteAction(number, false);
            }
        }

        private void ExecuteVassalTransferAction(Turn currentTurn, List<Command> currentCommands)
        {
            var commands = currentCommands
                .Where(c => c.Type == enCommandType.VassalTransfer && c.Status == enCommandStatus.ReadyToRun);
            foreach (var command in commands)
            {
                var isValid = _context.Organizations
                    .Any(o => o.Id == command.TargetOrganizationId &&
                        (command.TargetOrganizationId == command.OrganizationId || o.SuzerainId == command.OrganizationId));
                if (!isValid)
                {
                    _context.Remove(command);
                    continue;
                }
                var task = new VassalTransferAction(_context, currentTurn, command);
                number = task.ExecuteAction(number, true);
            }
        }

        private void ExecuteTaxAction(Turn currentTurn, List<Command> currentCommands)
        {
            var warCommands = currentCommands.Where(c => c.Type == enCommandType.CollectTax && c.Status == enCommandStatus.ReadyToRun);
            foreach (var command in warCommands)
            {
                var task = new TaxAction(_context, currentTurn, command);
                number = task.ExecuteAction(number, true);
            }
        }

        private void ExecuteVassalTaxAction(Turn currentTurn, List<Organization> organizations)
        {
            var vassals = organizations.Where(c => c.Suzerain != null);
            foreach (var organization in vassals)
            {
                var task = new VassalAction(_context, currentTurn, organization);
                number = task.ExecuteAction(number, false);
            }
        }

        private void ExecuteFortificationsMaintenanceAction(Turn currentTurn, params Organization[] organizations)
        {
            foreach (var organization in organizations)
            {
                var task = new FortificationsMaintenanceAction(_context, currentTurn, organization);
                number = task.ExecuteAction(number, false);
            }
        }

        private void ExecuteMaintenanceAction(Turn currentTurn, params Organization[] organizations)
        {
            foreach (var organization in organizations)
            {
                var task = new MaintenanceAction(_context, currentTurn, organization);
                number = task.ExecuteAction(number, false);
            }
        }

        private void ExecuteCorruptionAction(Turn currentTurn, params Organization[] allOrganizations)
        {
            var organizations = allOrganizations.Where(c => 
                c.User == null || c.User.LastActivityTime < DateTime.UtcNow - CorruptionParameters.CorruptionStartTime);
            foreach (var organization in organizations)
            {
                var task = new CorruptionAction(_context, currentTurn, organization);
                number = task.ExecuteAction(number, false);
            }
        }

        private void ExecuteMutinyAction(Turn currentTurn, params Organization[] organizations)
        {
            var bankrupts = organizations.Where(c => c.Warriors < 40);
            foreach (var organization in bankrupts)
            {
                var task = new MutinyAction(_context, currentTurn, organization);
                number = task.ExecuteAction(number, false);
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
