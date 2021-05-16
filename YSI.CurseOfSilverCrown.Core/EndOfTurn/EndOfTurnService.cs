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
using YSI.CurseOfSilverCrown.Core.Interfaces;

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
            var organizations = _context.Domains
                .Include(o => o.User)
                .Include(o => o.Suzerain)
                .Include(o => o.Vassals)
                .ToArray();
            CreatorCommandForNewTurn.CreateNewCommandsForOrganizations(_context, organizations);
        }

        public async Task<bool> Execute()
        {
            var currentTurn = _context.Turns.
                  Single(t => t.IsActive);

            number = 1;

            DeactivateCurrentTurn(currentTurn);

            var currentCommands = _context.Commands
                .Include(c => c.Domain)
                .Cast<ICommand>()
                .ToList();
            var currentUnits = _context.Units
                .Include(c => c.Domain)
                .Cast<ICommand>()
                .ToList();
            currentCommands.AddRange(currentUnits);
            var organizations = _context.Domains
                .Include(c => c.Commands)
                .Include(c => c.Units)
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
            ExecuteTaxAction(currentTurn, organizations);
            ExecuteIdlenessAction(currentTurn, currentCommands);
            ExecuteFortificationsMaintenanceAction(currentTurn, organizations);
            ExecuteMaintenanceAction(currentTurn, organizations);
            ExecuteCorruptionAction(currentTurn, organizations);
            ExecuteMutinyAction(currentTurn, organizations);


            _context.RemoveRange(_context.Commands);

            _context.RemoveRange(_context.Units.Where(c => c.Status == enCommandStatus.ForDelete || 
                c.Status == enCommandStatus.ReadyToSend || 
                c.Warriors <= 0));

            var newTurn = CreateNewTurn();
            CreatorCommandForNewTurn.CreateNewCommandsForOrganizations(_context, organizations);

            var changed = await _context.SaveChangesAsync();

            return changed > 0;
        }

        private void CheckSuzerainCommand(Domain[] organizations)
        {
            foreach (var organization in organizations)
            {
                if (organization.User != null && organization.User.LastActivityTime > DateTime.UtcNow - new TimeSpan(24, 0, 0))
                    continue;
                if (!organization.Units.Any(c => c.InitiatorDomainId == organization.SuzerainId))
                    continue;

                var botCommands = organization.Commands.Where(c => c.InitiatorDomainId == organization.Id);
                foreach (var botCommand in botCommands)
                    botCommand.Status = enCommandStatus.ForDelete;

                var botUnits = organization.Units.Where(c => c.InitiatorDomainId == organization.Id);
                foreach (var botCommand in botUnits)
                    botCommand.Status = enCommandStatus.ForDelete;

                var suzerainCommands = organization.Commands.Where(c => c.InitiatorDomainId == organization.SuzerainId);
                foreach (var suzerainCommand in suzerainCommands)
                {
                    suzerainCommand.Status = enCommandStatus.ReadyToRun;
                    suzerainCommand.InitiatorDomainId = organization.Id;
                    suzerainCommand.Initiator = organization;
                }

                var suzerainUnits = organization.Units.Where(c => c.InitiatorDomainId == organization.SuzerainId);
                foreach (var suzerainCommand in suzerainUnits)
                {
                    suzerainCommand.Status = enCommandStatus.ReadyToRun;
                    suzerainCommand.InitiatorDomainId = organization.Id;
                    suzerainCommand.Initiator = organization;
                }
            }
        }

        private void DeactivateCurrentTurn(Turn currentTurn)
        {            
            currentTurn.IsActive = false;
            _context.Update(currentTurn);
        }

        private void ExecuteGoldTransferAction(Turn currentTurn, List<ICommand> currentCommands)
        {
            var commands = currentCommands.Where(c => c.TypeInt == (int)enCommandType.GoldTransfer && c.Status == enCommandStatus.ReadyToRun);
            foreach (var command in commands)
            {
                var task = new GoldTransferAction(_context, currentTurn, command as Command);
                number = task.ExecuteAction(number, true);
            }
        }

        private void ExecuteGrowthAction(Turn currentTurn, List<ICommand> currentCommands)
        {
            var commands = currentCommands.Where(c => c.TypeInt == (int)enCommandType.Growth && c.Status == enCommandStatus.ReadyToRun);
            foreach (var command in commands)
            {
                if (command.Coffers < WarriorParameters.Price)
                {
                    _context.Remove(command);
                    continue;
                }
                var task = new GrowthAction(_context, currentTurn, command as Command);
                number = task.ExecuteAction(number, true);
            }
        }

        private void ExecuteInvestmentsAction(Turn currentTurn, List<ICommand> currentCommands)
        {
            var commands = currentCommands.Where(c => c.TypeInt == (int)enCommandType.Investments && c.Status == enCommandStatus.ReadyToRun);
            foreach (var command in commands)
            {
                if (command.Coffers <= 0)
                {
                    _context.Remove(command);
                    continue;
                }
                var task = new InvestmentsAction(_context, currentTurn, command as Command);
                number = task.ExecuteAction(number, true);
            }
        }

        private void ExecuteFortificationsAction(Turn currentTurn, List<ICommand> currentCommands)
        {
            var commands = currentCommands.Where(c => c.TypeInt == (int)enCommandType.Fortifications && c.Status == enCommandStatus.ReadyToRun);
            foreach (var command in commands)
            {
                if (command.Coffers <= 0)
                {
                    _context.Remove(command);
                    continue;
                }
                var task = new FortificationsAction(_context, currentTurn, command as Command);
                number = task.ExecuteAction(number, true);
            }
        }

        private void ExecuteIdlenessAction(Turn currentTurn, List<ICommand> currentCommands)
        {
            var idlenessCommands = currentCommands.Where(c => c.TypeInt == (int)enCommandType.Idleness && c.Status == enCommandStatus.ReadyToRun);
            foreach (var command in idlenessCommands)
            {
                if (command.Coffers <= 0)
                {
                    _context.Remove(command);
                    continue;
                }
                var task = new IdlenessAction(_context, currentTurn, command as Command);
                number = task.ExecuteAction(number, true);
            }
        }

        private void ExecuteRebelionAction(Turn currentTurn, List<ICommand> currentCommands)
        {
            var commands = currentCommands.Where(c => c.TypeInt == (int)enArmyCommandType.Rebellion && c.Status == enCommandStatus.ReadyToRun);
            foreach (var command in commands)
            {
                if (command.Warriors <= 0 || command.TargetDomainId != command.Domain.SuzerainId)                
                    continue;
                
                var task = new RebelionAction(_context, currentTurn, command as Unit);
                number = task.ExecuteAction(number, false);
            }
        }

        private void ExecuteWarAction(Turn currentTurn, List<ICommand> currentCommands)
        {
            var warCommands = currentCommands
                .Where(c => c.TypeInt == (int)enArmyCommandType.War && c.Status == enCommandStatus.ReadyToRun)
                .OrderBy(c => c.Warriors);
            foreach (var command in warCommands)
            {
                if (command.Warriors <= 0)
                    continue;

                var task = new WarAction(_context, currentTurn, command as Unit);
                number = task.ExecuteAction(number, false);
            }
        }

        private void ExecuteVassalTransferAction(Turn currentTurn, List<ICommand> currentCommands)
        {
            var commands = currentCommands
                .Where(c => c.TypeInt == (int)enCommandType.VassalTransfer && c.Status == enCommandStatus.ReadyToRun);
            foreach (var command in commands)
            {
                var isValid = _context.Domains
                    .Any(o => o.Id == command.TargetDomainId &&
                        (command.TargetDomainId == command.DomainId || o.SuzerainId == command.DomainId));
                if (!isValid)
                {
                    _context.Remove(command);
                    continue;
                }
                var task = new VassalTransferAction(_context, currentTurn, command as Command);
                number = task.ExecuteAction(number, true);
            }
        }

        private void ExecuteTaxAction(Turn currentTurn, params Domain[] organizations)
        {
            foreach (var organization in organizations)
            {
                var task = new TaxAction(_context, currentTurn, organization);
                number = task.ExecuteAction(number, false);
            }
        }

        private void ExecuteFortificationsMaintenanceAction(Turn currentTurn, params Domain[] organizations)
        {
            foreach (var organization in organizations)
            {
                var task = new FortificationsMaintenanceAction(_context, currentTurn, organization);
                number = task.ExecuteAction(number, false);
            }
        }

        private void ExecuteMaintenanceAction(Turn currentTurn, params Domain[] organizations)
        {
            foreach (var organization in organizations)
            {
                var task = new MaintenanceAction(_context, currentTurn, organization);
                number = task.ExecuteAction(number, false);
            }
        }

        private void ExecuteCorruptionAction(Turn currentTurn, params Domain[] allOrganizations)
        {
            var organizations = allOrganizations.Where(c => 
                c.User == null || c.User.LastActivityTime < DateTime.UtcNow - CorruptionParameters.CorruptionStartTime);
            foreach (var organization in organizations)
            {
                var task = new CorruptionAction(_context, currentTurn, organization);
                number = task.ExecuteAction(number, false);
            }
        }

        private void ExecuteMutinyAction(Turn currentTurn, params Domain[] organizations)
        {
            var bankrupts = organizations.Where(c => c.Units.Sum(u => u.Warriors) < 40);
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
