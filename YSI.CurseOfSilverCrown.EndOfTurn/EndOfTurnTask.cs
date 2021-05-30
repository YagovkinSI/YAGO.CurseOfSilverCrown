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
using YSI.CurseOfSilverCrown.Core;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.EndOfTurn
{
    public class EndOfTurnTask
    {
        private ApplicationDbContext Context;
        private Turn CurrentTurn;

        private int eventNumber;
        private const int SubTurnCount = 2;

        public EndOfTurnTask(ApplicationDbContext context)
        {
            Context = context;
        }

        public Response<bool> Execute()
        {
            try
            {
                DeactivateCurrentTurn();
                Prepare();
                RunUnits();
                RunCommands();
                RetrearUnits();
                PrepareForNewTurn();
                CreateNewTurn();
                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                return new Response<bool>("Ошибка во время прокрутки", ex);
            }
        }

        private void RunCommands()
        {
            var runCommands = Context.Commands.ToList();
            var organizations = Context.Domains.ToArray();

            ExecuteRebelionAction(CurrentTurn, runCommands);
            ExecuteVassalTransferAction(CurrentTurn, runCommands);
            ExecuteGoldTransferAction(CurrentTurn, runCommands);
            ExecuteGrowthAction(CurrentTurn, runCommands);
            ExecuteInvestmentsAction(CurrentTurn, runCommands);
            ExecuteFortificationsAction(CurrentTurn, runCommands);
            ExecuteTaxAction(CurrentTurn, organizations);
            ExecuteIdlenessAction(CurrentTurn, runCommands);
            ExecuteFortificationsMaintenanceAction(CurrentTurn, organizations);
            ExecuteMaintenanceAction(CurrentTurn, organizations);

            ExecuteMutinyAction(CurrentTurn, organizations);

            Context.RemoveRange(runCommands);

            Context.SaveChanges();
        }

        private void RunUnits()
        {
            var runUnitIds = Context.Units
                .OrderBy(u => u.Warriors)
                .Select(u => u.Id)
                .ToList();

            //Защита на месте уже защищает
            foreach (var unitId in runUnitIds)
            {
                var unit = Context.Units.Find(unitId);
                if (unit.Type == enArmyCommandType.WarSupportDefense &&
                    unit.TargetDomainId == unit.PositionDomainId || 
                    unit.Type == enArmyCommandType.CollectTax && 
                    unit.DomainId == unit.PositionDomainId)
                {
                    unit.Status = enCommandStatus.Complited;
                    Context.Update(unit);
                }
            }
            Context.SaveChanges();

            for (var subTurn = 0; subTurn < SubTurnCount; subTurn++)
            {
                foreach (var unitId in runUnitIds)
                {
                    var unit = Context.Units
                        .Include(u => u.Domain)
                        .Single(u => u.Id == unitId);
                    if (unit.Status == enCommandStatus.Complited ||
                        unit.ActionPoints < WarConstants.ActionPointsFullCount - subTurn * WarConstants.ActionPointForMoveWarriors)
                        continue;

                    switch (unit.Type)
                    {
                        case enArmyCommandType.CollectTax:
                            if (unit.PositionDomainId != unit.DomainId)
                            {
                                var task = new UnitMoveAction(Context, CurrentTurn, unitId);
                                eventNumber = task.ExecuteAction(eventNumber);
                                Context.SaveChanges();
                            }
                            if (unit.PositionDomainId == unit.DomainId)
                            {
                                unit.Status = enCommandStatus.Complited;
                                Context.Update(unit);
                                Context.SaveChanges();
                            }
                            break;
                        case enArmyCommandType.War:
                            if (!RouteHelper.IsNeighbors(Context, unit.PositionDomainId.Value, unit.TargetDomainId.Value))
                            {
                                var task = new UnitMoveAction(Context, CurrentTurn, unit.Id);
                                eventNumber = task.ExecuteAction(eventNumber);
                                Context.SaveChanges();
                            }
                            else
                            {
                                var task = new WarAction(Context, CurrentTurn, unit.Id);
                                eventNumber = task.ExecuteAction(eventNumber);
                                Context.SaveChanges();
                                if (task.IsVictory)
                                {
                                    var retreats = Context.Units
                                        .Where(u => u.Status == enCommandStatus.Retreat)
                                        .ToList();
                                    foreach (var retreatUnit in retreats)
                                    {
                                        var retreatTask = new RetreatAction(Context, CurrentTurn, retreatUnit.Id);
                                        eventNumber = retreatTask.ExecuteAction(eventNumber);
                                        Context.SaveChanges();
                                    }
                                }
                            }
                            break;
                        case enArmyCommandType.WarSupportAttack:
                            if (!RouteHelper.IsNeighbors(Context, unit.PositionDomainId.Value, unit.TargetDomainId.Value))
                            {
                                var task = new UnitMoveAction(Context, CurrentTurn, unit.Id);
                                eventNumber = task.ExecuteAction(eventNumber);
                                Context.SaveChanges();
                            }
                            else
                            {
                                unit.Status = enCommandStatus.Complited;
                                Context.Update(unit);
                                Context.SaveChanges();
                            }
                            break;
                        case enArmyCommandType.WarSupportDefense:
                            if (unit.PositionDomainId != unit.TargetDomainId)
                            {
                                var task = new UnitMoveAction(Context, CurrentTurn, unit.Id);
                                eventNumber = task.ExecuteAction(eventNumber);
                                Context.SaveChanges();
                            }
                            if (unit.PositionDomainId == unit.TargetDomainId)
                            {
                                unit = Context.Units.Find(unit.Id);
                                unit.Status = enCommandStatus.Complited;
                                Context.Update(unit);
                                Context.SaveChanges();
                            }
                            break;
                        case enArmyCommandType.ForDelete:
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    unit.ActionPoints -= WarConstants.ActionPointForMoveWarriors;
                    Context.Update(unit);
                    Context.SaveChanges();
                }
            }

            //Кто не успел, тот опоздал
            foreach (var unitId in runUnitIds)
            {
                var unit = Context.Units.Find(unitId);
                unit.Status = enCommandStatus.Complited;
                Context.Update(unit);
            }            

            var unitForDelete = Context.Units.Where(c => c.Warriors <= 0 || c.Status == enCommandStatus.Destroyed);
            Context.RemoveRange(unitForDelete);

            Context.SaveChanges();
        }

        private void RetrearUnits()
        {
            var runUnitIds = Context.Units
                .OrderBy(u => u.Warriors)
                .Select(u => u.Id)
                .ToList();

            //Отступаем или уничтожаемся
            foreach (var unitId in runUnitIds)
            {
                var unit = Context.Units.Find(unitId);
                if (unit.PositionDomainId == unit.DomainId)
                    continue;

                var unitDomain = Context.Domains.Find(unit.DomainId);
                var unitPosition = Context.Domains.Find(unit.PositionDomainId.Value);
                if (KingdomHelper.IsSameKingdoms(Context.Domains, unitDomain, unitPosition))
                    continue;

                unit.Status = enCommandStatus.Retreat;
                Context.Update(unit);
                var task = new RetreatAction(Context, CurrentTurn, unit.Id);
                eventNumber = task.ExecuteAction(eventNumber);
            }

            var unitForDelete = Context.Units.Where(c => c.Warriors <= 0 || c.Status == enCommandStatus.Destroyed);
            Context.RemoveRange(unitForDelete);

            Context.SaveChanges();
        }

        private void PrepareForNewTurn()
        {
            PrepareUnitsForNewTurn();
            PrepareCommandsForNewTurn();
        }

        private void PrepareCommandsForNewTurn()
        {
            var doaminMainArray = Context.GetAllDomainMain().Result.ToArray();
            CreatorCommandForNewTurn.CreateNewCommandsForOrganizations(Context, doaminMainArray);
            Context.SaveChanges();
        }

        private void PrepareUnitsForNewTurn()
        {
            var runUnits = Context.Units.ToList();

            var unitCompleted = runUnits.Where(c => c.Status == enCommandStatus.Complited);
            foreach (var unit in unitCompleted)
            {
                if (unit.Type != enArmyCommandType.CollectTax && unit.Type != enArmyCommandType.WarSupportDefense)
                {
                    unit.Type = enArmyCommandType.WarSupportDefense;
                    unit.Target2DomainId = null;
                    unit.TargetDomainId = unit.DomainId;
                }
                unit.Status = enCommandStatus.ReadyToMove;
                unit.ActionPoints = WarConstants.ActionPointsFullCount;
            }
            Context.UpdateRange(unitCompleted);

            Context.SaveChanges();
        }

        private void Prepare()
        {
            PrepareCommands();
            PrepareUnits();
        }

        public void CreateCommands()
        {
            var organizations = Context.GetAllDomainMain().Result;
            CreatorCommandForNewTurn.CreateNewCommandsForOrganizations(Context, organizations.ToArray());
        }

        private void PrepareCommands()
        {
            var domains = Context.Domains
                .Include(o => o.User)
                .Include(o => o.Commands)
                .ToList();
            foreach (var domain in domains)
            {
                var initiatorRunId = 0;
                var groups = domain.Commands
                    .GroupBy(c => c.InitiatorDomainId);
                if (groups.Count() > 1)
                {
                    var domainIsActive = domain.User != null &&
                                         domain.User.LastActivityTime > DateTime.UtcNow - new TimeSpan(24, 0, 0);
                    initiatorRunId = domainIsActive
                        ? domain.Id
                        : domain.SuzerainId.Value;
                }
                else
                    initiatorRunId = domain.Id;

                var groupsForDelete = groups.Where(g => g.Key != initiatorRunId);
                foreach (var group in groupsForDelete)
                    Context.RemoveRange(group.ToList());

                var groupForRun = groups.Single(g => g.Key == initiatorRunId);
                foreach (var command in groupForRun)                
                    command.InitiatorDomainId = command.DomainId;                
                Context.UpdateRange(groupForRun);
                Context.SaveChanges();
            }
        }

        private void PrepareUnits()
        {
            var domains = Context.Domains
                .Include(o => o.User)
                .Include(o => o.Units)
                .ToList();
            foreach (var domain in domains)
            {
                var initiatorRunId = 0;
                var groups = domain.Units
                    .GroupBy(c => c.InitiatorDomainId);
                if (groups.Count() > 1)
                {
                    var domainIsActive = domain.User != null &&
                                         domain.User.LastActivityTime > DateTime.UtcNow - new TimeSpan(24, 0, 0);
                    initiatorRunId = domainIsActive
                        ? domain.Id
                        : domain.SuzerainId.Value;
                }
                else
                    initiatorRunId = domain.Id;

                var groupsForDelete = groups.Where(g => g.Key != initiatorRunId);
                foreach (var group in groupsForDelete)
                    Context.RemoveRange(group.ToList());

                var groupForRun = groups.Single(g => g.Key == initiatorRunId);
                foreach (var unit in groupForRun)
                    unit.InitiatorDomainId = unit.DomainId;
                Context.UpdateRange(groupForRun);
                Context.SaveChanges();
            }
        }

        private void DeactivateCurrentTurn()
        {
            CurrentTurn = Context.Turns.
                  SingleOrDefault(t => t.IsActive);

            if (CurrentTurn != null)
            {
                CurrentTurn.IsActive = false;
                Context.Update(CurrentTurn);
                Context.SaveChanges();
            }
            else
                CurrentTurn = Context.Turns
                    .OrderByDescending(t => t.Id)
                    .First();
        }

        private void ExecuteGoldTransferAction(Turn currentTurn, IEnumerable<Command> currentCommands)
        {
            foreach (var command in currentCommands)
            {
                var task = new GoldTransferAction(Context, currentTurn, command);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteGrowthAction(Turn currentTurn, IEnumerable<Command> currentCommands)
        {
            foreach (var command in currentCommands)
            {
                var task = new GrowthAction(Context, currentTurn, command);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteInvestmentsAction(Turn currentTurn, IEnumerable<Command> currentCommands)
        {
            foreach (var command in currentCommands)
            {
                var task = new InvestmentsAction(Context, currentTurn, command);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteFortificationsAction(Turn currentTurn, IEnumerable<Command> currentCommands)
        {
            foreach (var command in currentCommands)
            {
                var task = new FortificationsAction(Context, currentTurn, command);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteIdlenessAction(Turn currentTurn, IEnumerable<Command> currentCommands)
        {
            foreach (var command in currentCommands)
            {
                var task = new IdlenessAction(Context, currentTurn, command);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteRebelionAction(Turn currentTurn, IEnumerable<ICommand> currentCommands)
        {
            foreach (var command in currentCommands)
            {
                var task = new RebelionAction(Context, currentTurn, command as Command);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteVassalTransferAction(Turn currentTurn, IEnumerable<ICommand> currentCommands)
        {
            foreach (var command in currentCommands)
            {
                var task = new VassalTransferAction(Context, currentTurn, command as Command);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteTaxAction(Turn currentTurn, params Domain[] organizations)
        {
            foreach (var organization in organizations)
            {
                var task = new TaxAction(Context, currentTurn, organization);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteFortificationsMaintenanceAction(Turn currentTurn, params Domain[] organizations)
        {
            foreach (var organization in organizations)
            {
                var task = new FortificationsMaintenanceAction(Context, currentTurn, organization);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteMaintenanceAction(Turn currentTurn, params Domain[] organizations)
        {
            foreach (var organization in organizations)
            {
                var task = new MaintenanceAction(Context, currentTurn, organization);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void ExecuteMutinyAction(Turn currentTurn, params Domain[] organizations)
        {
            foreach (var organization in organizations)
            {
                var task = new MutinyAction(Context, currentTurn, organization);
                eventNumber = task.ExecuteAction(eventNumber);
            }
        }

        private void CreateNewTurn()
        {
            var newTurn = new Turn
            {
                IsActive = true,
                Started = DateTime.UtcNow
            };
            Context.Add(newTurn);
            Context.SaveChanges();
        }
    }
}
