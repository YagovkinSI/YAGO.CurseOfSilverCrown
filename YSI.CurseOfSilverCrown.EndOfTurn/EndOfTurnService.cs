using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.EndOfTurn.Actions;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Interfaces;
using YSI.CurseOfSilverCrown.Core.BL.Models;
using YSI.CurseOfSilverCrown.Core.BL.Models.Main;

namespace YSI.CurseOfSilverCrown.EndOfTurn
{
    public class EndOfTurnService
    {
        private ApplicationDbContext _context;

        private int eventNumber;
        private const int SubTurnCount = 2;

        public EndOfTurnService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateCommands()
        {
            var organizations = _context.GetAllDomainMain().Result;
            CreatorCommandForNewTurn.CreateNewCommandsForOrganizations(_context, organizations.ToArray());
        }

        public async Task<bool> Execute()
        {
            var currentTurn = _context.Turns.
                  Single(t => t.IsActive);

            DeactivateCurrentTurn(currentTurn);

            var organizations = await GetOrganizations()
                .ToArrayAsync();

            CheckSuzerainCommand(organizations);

            var allCommands = await _context.Commands
                .ToArrayAsync();
            var allUnits = await _context.Units
                .Include(u => u.Domain)
                .ToArrayAsync();


            var runCommands = allCommands
                .Where(c => c.Status == enCommandStatus.ReadyToRun &&
                    c.InitiatorDomainId == c.DomainId);
            var runUnits = allUnits
                .Where(u => u.Status == enCommandStatus.ReadyToRun)
                .OrderBy(u => u.Warriors);

            for (var subTurn = 0; subTurn < SubTurnCount; subTurn++)
            {
                foreach (var unit in runUnits)
                {
                    if (unit.Status == enCommandStatus.Complited)
                        continue;

                    switch(unit.Type)
                    {
                        case enArmyCommandType.ForDelete:
                            break;
                        case enArmyCommandType.CollectTax:
                            if (unit.PositionDomainId != unit.DomainId)
                            {
                                var task = new UnitMoveAction(_context, currentTurn, unit);
                                eventNumber = task.ExecuteAction(eventNumber);
                            }
                            if (unit.PositionDomainId == unit.DomainId)
                                unit.Status = enCommandStatus.Complited;
                            break;
                        case enArmyCommandType.Rebellion:
                            if (unit.PositionDomainId != unit.Domain.SuzerainId)
                            {
                                var task = new UnitMoveAction(_context, currentTurn, unit);
                                eventNumber = task.ExecuteAction(eventNumber);
                            }
                            if (unit.PositionDomainId == unit.Domain.SuzerainId)
                            {
                                var task = new RebelionAction(_context, currentTurn, unit);
                                eventNumber = task.ExecuteAction(eventNumber);
                                unit.Status = enCommandStatus.Complited;
                            }
                            break;
                        case enArmyCommandType.War:
                            if (unit.PositionDomainId != unit.TargetDomainId)
                            {
                                var task = new UnitMoveAction(_context, currentTurn, unit);
                                eventNumber = task.ExecuteAction(eventNumber);
                            }
                            if (unit.PositionDomainId == unit.TargetDomainId)
                            {
                                var task = new WarAction(_context, currentTurn, unit);
                                eventNumber = task.ExecuteAction(eventNumber);
                                unit.Status = enCommandStatus.Complited;
                            }
                            break;
                        case enArmyCommandType.WarSupportAttack:
                            if (unit.PositionDomainId != unit.TargetDomainId.Value)
                            {
                                var task = new UnitMoveAction(_context, currentTurn, unit);
                                eventNumber = task.ExecuteAction(eventNumber);
                            }
                            if (unit.PositionDomainId == unit.TargetDomainId.Value)
                            {
                                unit.Status = enCommandStatus.Complited;
                            }
                            break;
                        case enArmyCommandType.WarSupportDefense:
                            if (unit.PositionDomainId != unit.TargetDomainId.Value)
                            {
                                var task = new UnitMoveAction(_context, currentTurn, unit);
                                eventNumber = task.ExecuteAction(eventNumber);
                            }
                            else
                            {
                                unit.Status = enCommandStatus.Complited;
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                _context.UpdateRange(runUnits);
            }

            ExecuteVassalTransferAction(currentTurn, runCommands);
            ExecuteGoldTransferAction(currentTurn, runCommands);
            ExecuteGrowthAction(currentTurn, runCommands);
            ExecuteInvestmentsAction(currentTurn, runCommands);
            ExecuteFortificationsAction(currentTurn, runCommands);
            ExecuteTaxAction(currentTurn, organizations);
            ExecuteIdlenessAction(currentTurn, runCommands);
            ExecuteFortificationsMaintenanceAction(currentTurn, organizations);
            ExecuteMaintenanceAction(currentTurn, organizations);
            ExecuteCorruptionAction(currentTurn, organizations);
            ExecuteMutinyAction(currentTurn, organizations);

            _context.RemoveRange(allCommands);

            var unitForDelete = allUnits.Where(c => c.Status == enCommandStatus.ForDelete ||
                c.Status == enCommandStatus.ReadyToSend ||
                c.Warriors <= 0);
            _context.RemoveRange(unitForDelete);

            var unitCompleted = allUnits.Where(c => c.Status == enCommandStatus.Complited);
            foreach (var unit in unitCompleted)
            {
                if (unit.Type != enArmyCommandType.CollectTax && unit.Type != enArmyCommandType.WarSupportDefense)
                {
                    unit.Type = enArmyCommandType.WarSupportDefense;
                    unit.Target2DomainId = null;
                    unit.TargetDomainId = unit.DomainId;
                }
                unit.Status = enCommandStatus.ReadyToRun;
            }
            _context.UpdateRange(unitCompleted);

            var newTurn = CreateNewTurn();
            var doaminMainArray = organizations.Select(d => new DomainMain(d)).ToArray();
            CreatorCommandForNewTurn.CreateNewCommandsForOrganizations(_context, doaminMainArray);

            var changed = await _context.SaveChangesAsync();

            return changed > 0;
        }

        private IQueryable<Domain> GetOrganizations()
        {
            return _context.Domains
                .Include(c => c.Commands)
                .Include(c => c.Units)
                .Include(o => o.User)
                .Include(o => o.Suzerain)
                .Include(o => o.Vassals);
        }

        private void CheckSuzerainCommand(IEnumerable<Domain> organizations)
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

        private void ExecuteGoldTransferAction(Turn currentTurn, IEnumerable<Command> currentCommands)
        {
            foreach (var command in currentCommands)
            {
                var task = new GoldTransferAction(_context, currentTurn, command);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteGrowthAction(Turn currentTurn, IEnumerable<Command> currentCommands)
        {
            foreach (var command in currentCommands)
            {
                var task = new GrowthAction(_context, currentTurn, command);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteInvestmentsAction(Turn currentTurn, IEnumerable<Command> currentCommands)
        {
            foreach (var command in currentCommands)
            {
                var task = new InvestmentsAction(_context, currentTurn, command);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteFortificationsAction(Turn currentTurn, IEnumerable<Command> currentCommands)
        {
            foreach (var command in currentCommands)
            {
                var task = new FortificationsAction(_context, currentTurn, command);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteIdlenessAction(Turn currentTurn, IEnumerable<Command> currentCommands)
        {
            foreach (var command in currentCommands)
            {
                var task = new IdlenessAction(_context, currentTurn, command);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteVassalTransferAction(Turn currentTurn, IEnumerable<ICommand> currentCommands)
        {
            foreach (var command in currentCommands)
            {
                var task = new VassalTransferAction(_context, currentTurn, command as Command);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteTaxAction(Turn currentTurn, params Domain[] organizations)
        {
            foreach (var organization in organizations)
            {
                var task = new TaxAction(_context, currentTurn, organization);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteFortificationsMaintenanceAction(Turn currentTurn, params Domain[] organizations)
        {
            foreach (var organization in organizations)
            {
                var task = new FortificationsMaintenanceAction(_context, currentTurn, organization);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteMaintenanceAction(Turn currentTurn, params Domain[] organizations)
        {
            foreach (var organization in organizations)
            {
                var task = new MaintenanceAction(_context, currentTurn, organization);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteCorruptionAction(Turn currentTurn, params Domain[] allOrganizations)
        {
            foreach (var organization in allOrganizations)
            {
                var task = new CorruptionAction(_context, currentTurn, organization);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteMutinyAction(Turn currentTurn, params Domain[] organizations)
        {
            foreach (var organization in organizations)
            {
                var task = new MutinyAction(_context, currentTurn, organization);
                eventNumber = task.ExecuteAction(eventNumber);
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
