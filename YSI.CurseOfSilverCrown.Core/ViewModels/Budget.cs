using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Commands;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Interfaces;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.ViewModels
{
    public class Budget
    {
        private const int ExpectedLossesEvery = 10;

        public List<LineOfBudget> Lines { get; set; } = new List<LineOfBudget>();
        public Domain Organization { get; private set; }
        public ApplicationDbContext Context { get; }

        private List<Func<Domain, List<ICommand>, IEnumerable<LineOfBudget>>> LineFunctions => new()
        {
            GetCurrent,

            WarSupportDefense,
            War,
            WarSupportAttack,
            GetGrowth,
            GetInvestments,
            GetFortifications,
            GetAditionalTax,
            GetInvestmentProfit,
            VassalTax,
            GetSuzerainTax,
            GetMaintenance,
            GetMaintenanceFortifications,
            GetGoldTransfers,
            VassalTransfers,
            Rebelion,

            GetNotAllocated,
            GetTotal
        };

        public Budget(ApplicationDbContext context, Domain organization, int initiatorId)
        {
            Context = context;
            var allCommand = GetAllCommandsAsync(organization, initiatorId, context).Result;
            Init(organization, allCommand);
        }

        private async Task<IEnumerable<ICommand>> GetAllCommandsAsync(Domain organization, int initiatorId,
            ApplicationDbContext context)
        {
            var allCommands = await context.Commands
                .Include(c => c.Domain)
                .Include(c => c.Target)
                .Include(c => c.Target2)
                .Where(c => c.DomainId == organization.Id &&
                    c.InitiatorPersonId == initiatorId)
                .Cast<ICommand>()
                .ToListAsync();

            var units = await context.Units
                .Include(c => c.Domain)
                .Include(c => c.Target)
                .Include(c => c.Target2)
                .Where(c => c.DomainId == organization.Id &&
                    c.InitiatorPersonId == initiatorId)
                .Cast<ICommand>()
                .ToListAsync();

            allCommands.AddRange(units);
            return allCommands;
        }

        private void Init(Domain domain, IEnumerable<ICommand> allCommand)
        {
            Lines = new List<LineOfBudget>();
            Organization = domain;
            foreach (var func in LineFunctions)
                Lines.AddRange(func(domain, allCommand.ToList()));
        }

        private IEnumerable<LineOfBudget> GetCurrent(Domain domain, List<ICommand> organizationCommands)
        {
            var currentWarriors = domain.WarriorCount;
            var defense = WarConstants.DefaultDefenseWarrioirs *
                FortificationsHelper.GetWariorDefenseCoeficient(WarConstants.WariorDefenseSupport, domain.Fortifications);
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.Current,
                    CommandSourceTable = enCommandSourceTable.NotCommand,
                    Coffers = new ParameterChanging<int?>(domain.Coffers, domain.Coffers),
                    Warriors = new ParameterChanging<int?>(currentWarriors, currentWarriors),
                    Investments = new ParameterChanging<int?>(domain.InvestmentsShowed, domain.InvestmentsShowed),
                    Defense = new ParameterChanging<double?>(defense, defense),
                    Descripton = "Имеется на начало сезона"
                }
            };
        }

        private IEnumerable<LineOfBudget> GetMaintenance(Domain organization, List<ICommand> organizationCommands)
        {
            var growth = organizationCommands.Single(c => c.TypeInt == (int)enCommandType.Growth);
            var currentWarriors = organization.WarriorCount;
            var newWarriors = growth.Coffers / WarriorParameters.Price;
            var expectedLosses = organizationCommands
                .Where(c => c.TypeInt == (int)enArmyCommandType.War || c.TypeInt == (int)enArmyCommandType.WarSupportAttack)
                .Sum(w => w.Warriors / ExpectedLossesEvery);
            var expectedWarriorsForMaintenance = currentWarriors + newWarriors - expectedLosses;
            var expectedCoffers = -expectedWarriorsForMaintenance * WarriorParameters.Maintenance;
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.Maintenance,
                    CommandSourceTable = enCommandSourceTable.NotCommand,
                    Coffers = new ParameterChanging<int?>(null, expectedCoffers),
                    Descripton = "Ожидаемые затраты на содержание воинов"
                }
            };
        }

        private IEnumerable<LineOfBudget> GetMaintenanceFortifications(Domain organization, List<ICommand> organizationCommands)
        {
            var newFortifications = organizationCommands.Single(c => c.TypeInt == (int)enCommandType.Fortifications).Coffers;
            var currentFortifications = organization.Fortifications;
            var expectedWarriorsForMaintenance = currentFortifications + newFortifications;
            var expectedCoffers = -(int)Math.Round(expectedWarriorsForMaintenance * FortificationsParameters.MaintenancePercent);
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.FortificationsMaintenance,
                    CommandSourceTable = enCommandSourceTable.NotCommand,
                    Coffers = new ParameterChanging<int?>(null, expectedCoffers),
                    Descripton = "Затраты на содержание укреплений"
                }
            };
        }

        private IEnumerable<LineOfBudget> GetGrowth(Domain organization, List<ICommand> organizationCommands)
        {
            var command = organizationCommands.Single(c => c.TypeInt == (int)enCommandType.Growth);
            var expectedWarriorios = command.Coffers / WarriorParameters.Price;
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.Growth,
                    CommandSourceTable = enCommandSourceTable.Commands,
                    Coffers = new ParameterChanging<int?>(-command.Coffers, -command.Coffers),
                    Warriors = new ParameterChanging<int?>(null, expectedWarriorios),
                    Descripton = "Затраты на набор новых воинов",
                    Editable = true,
                    CommandId = command.Id
                }
            };
        }

        private IEnumerable<LineOfBudget> GetInvestments(Domain organization, List<ICommand> organizationCommands)
        {
            var command = organizationCommands.Single(c => c.TypeInt == (int)enCommandType.Investments);
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.Investments,
                    CommandSourceTable = enCommandSourceTable.Commands,
                    Coffers = new ParameterChanging<int?>(-command.Coffers, -command.Coffers),
                    Investments = new ParameterChanging<int?>(null, command.Coffers),
                    Descripton = "Вложения средств в имущество владения",
                    Editable = true,
                    CommandId = command.Id
                }
            };
        }

        private IEnumerable<LineOfBudget> GetFortifications(Domain organization, List<ICommand> organizationCommands)
        {
            var command = organizationCommands.Single(c => c.TypeInt == (int)enCommandType.Fortifications);
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.Fortifications,
                    CommandSourceTable = enCommandSourceTable.Commands,
                    Coffers = new ParameterChanging<int?>(-command.Coffers, -command.Coffers),
                    Fortifications = new ParameterChanging<int?>(null, command.Coffers),
                    Descripton = "Вложения средств в постройку укреплений",
                    Editable = true,
                    CommandId = command.Id
                }
            };
        }

        private IEnumerable<LineOfBudget> GetInvestmentProfit(Domain organization, List<ICommand> organizationCommands)
        {
            var investments = organizationCommands.Single(c => c.TypeInt == (int)enCommandType.Investments);
            var expectedCoffers = InvestmentsHelper.GetInvestmentTax(organization.Investments + investments.Coffers);
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.InvestmentProfit,
                    CommandSourceTable = enCommandSourceTable.Commands,
                    Coffers = new ParameterChanging<int?>(null, expectedCoffers),
                    Descripton = "Основной налог"
                }
            };
        }

        private IEnumerable<LineOfBudget> GetAditionalTax(Domain organization, List<ICommand> organizationCommands)
        {
            var command = organizationCommands.SingleOrDefault(c => c.TypeInt == (int)enArmyCommandType.CollectTax);
            if (command == null)
                return new LineOfBudget[0];

            var additoinalWarriors = command.Warriors;
            var expectedCoffers = Constants.GetAdditionalTax(additoinalWarriors);
            var expectedDefense = additoinalWarriors *
                FortificationsHelper.GetWariorDefenseCoeficient(WarConstants.WariorDefenseTax, organization.Fortifications);
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.AditionalTax,
                    CommandSourceTable = enCommandSourceTable.Units,
                    Warriors = new ParameterChanging<int?>(-additoinalWarriors, null),
                    Coffers = new ParameterChanging<int?>(null, expectedCoffers),
                    Defense = new ParameterChanging<double?>(null, expectedDefense),
                    Descripton = "Временный роспуск отряда",
                    Editable = true,
                    CommandId = command.Id
                }
            };
        }

        private IEnumerable<LineOfBudget> VassalTax(Domain organization, List<ICommand> organizationCommands)
        {
            var vassals = organization.Vassals;
            return vassals.Select(vassal => new LineOfBudget
            {
                Type = enLineOfBudgetType.VassalTax,
                CommandSourceTable = enCommandSourceTable.NotCommand,
                Coffers = new ParameterChanging<int?>(null, InvestmentsHelper.GetInvestmentTax(vassal.Investments)),
                Descripton = $"Получение налогов от вассала {vassal.Name}"
            });
        }

        private IEnumerable<LineOfBudget> GetSuzerainTax(Domain organization, List<ICommand> organizationCommands)
        {
            if (organization.Suzerain == null)
                return Array.Empty<LineOfBudget>();

            var additoinalWarriors = organizationCommands.SingleOrDefault(c => c.TypeInt == (int)enArmyCommandType.CollectTax)?.Warriors ?? 0;
            var investments = organizationCommands.Single(c => c.TypeInt == (int)enCommandType.Investments);
            var allIncome = Constants.GetAdditionalTax(additoinalWarriors) +
                InvestmentsHelper.GetInvestmentTax(organization.Investments + investments.Coffers);
            var expectedCoffers = (int)(-Math.Round(allIncome * Constants.BaseVassalTax));

            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.SuzerainTax,
                    CommandSourceTable = enCommandSourceTable.NotCommand,
                    Coffers = new ParameterChanging<int?>(null, expectedCoffers),
                    Descripton = $"Передача налога сюзерену в {organization.Suzerain.Name}"
                }
            };
        }

        private IEnumerable<LineOfBudget> War(Domain organization, List<ICommand> organizationCommands)
        {
            var commands = organizationCommands.Where(c => c.TypeInt == (int)enArmyCommandType.War);
            return commands.Select(command => new LineOfBudget
            {
                Type = enLineOfBudgetType.War,
                CommandSourceTable = enCommandSourceTable.Units,
                Warriors = new ParameterChanging<int?>(-command.Warriors, -command.Warriors / ExpectedLossesEvery),
                Descripton = $"Нападение на {command.Target?.Name}",
                Editable = true,
                Deleteable = true,
                CommandId = command.Id
            });
        }

        private IEnumerable<LineOfBudget> WarSupportAttack(Domain organization, List<ICommand> organizationCommands)
        {
            var commands = organizationCommands.Where(c => c.TypeInt == (int)enArmyCommandType.WarSupportAttack);
            return commands.Select(command => new LineOfBudget
            {
                Type = enLineOfBudgetType.WarSupportAtack,
                CommandSourceTable = enCommandSourceTable.Units,
                Warriors = new ParameterChanging<int?>(-command.Warriors, -command.Warriors / ExpectedLossesEvery),
                Descripton = $"Помощь владению {command.Target2?.Name} в нападении на {command.Target?.Name}",
                Editable = true,
                Deleteable = true,
                CommandId = command.Id
            });
        }

        private IEnumerable<LineOfBudget> WarSupportDefense(Domain organization, List<ICommand> organizationCommands)
        {
            var commands = organizationCommands.Where(c => c.TypeInt == (int)enArmyCommandType.WarSupportDefense);
            var lines = new List<LineOfBudget>();
            foreach (var command in commands)
            {
                var expectedDefense = command.TargetDomainId == command.DomainId
                    ? command.Warriors *
                        FortificationsHelper.GetWariorDefenseCoeficient(WarConstants.WariorDefenseSupport, organization.Fortifications)
                    : (double?)null;
                var line = new LineOfBudget
                {
                    Type = enLineOfBudgetType.WarSupportDefense,
                    CommandSourceTable = enCommandSourceTable.Units,
                    Warriors = new ParameterChanging<int?>(-command.Warriors, null),
                    Defense = new ParameterChanging<double?>(null, expectedDefense),
                    Descripton = $"Защита владения {command.Target?.Name}",
                    Editable = true,
                    Deleteable = true,
                    CommandId = command.Id
                };
                lines.Add(line);
            }
            return lines;
        }

        private IEnumerable<LineOfBudget> VassalTransfers(Domain organization, List<ICommand> organizationCommands)
        {
            var commands = organizationCommands.Where(c => c.TypeInt == (int)enCommandType.VassalTransfer);
            return commands.Select(command => new LineOfBudget
            {
                Type = enLineOfBudgetType.VassalTransfer,
                CommandSourceTable = enCommandSourceTable.Commands,
                Descripton = command.TargetDomainId == command.Target2DomainId
                    ? $"Освобождение владения {command.Target.Name} от вассальной клятвы"
                    : command.DomainId == command.TargetDomainId
                        ? $"Добровольная присяга провиции {command.Target2.Name}"
                        : $"Передача владения {command.Target.Name} под покровительство провинции {command.Target2.Name}",
                Editable = true,
                Deleteable = true,
                CommandId = command.Id
            });
        }

        private IEnumerable<LineOfBudget> Rebelion(Domain organization, List<ICommand> organizationCommands)
        {
            var command = organizationCommands.SingleOrDefault(c => c.TypeInt == (int)enCommandType.Rebellion);
            if (command == null)
                return new LineOfBudget[0];

            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.Rebelion,
                    CommandSourceTable = enCommandSourceTable.Commands,
                    Descripton = "Объявление независимости",
                    Editable = false,
                    Deleteable = true,
                    CommandId = command.Id
                }
            };
        }

        private IEnumerable<LineOfBudget> GetGoldTransfers(Domain organization, List<ICommand> organizationCommands)
        {
            var commands = organizationCommands.Where(c => c.TypeInt == (int)enCommandType.GoldTransfer);
            return commands.Select(command => new LineOfBudget
            {
                Type = enLineOfBudgetType.GoldTransfer,
                CommandSourceTable = enCommandSourceTable.Commands,
                Descripton = $"Передача золота во владение {command.Target.Name}",
                Coffers = new ParameterChanging<int?>(-command.Coffers, -command.Coffers),
                Editable = true,
                Deleteable = true,
                CommandId = command.Id
            });
        }

        private IEnumerable<LineOfBudget> GetNotAllocated(Domain organization, List<ICommand> organizationCommands)
        {
            var coffers = Lines.Sum(l => l.Coffers?.CurrentValue);
            var warriors = Lines.Sum(l => l.Warriors?.CurrentValue);
            var exceptedDefense = Lines.Sum(l => l.Warriors?.CurrentValue) *
                FortificationsHelper.GetWariorDefenseCoeficient(WarConstants.WariorDefenseTax, organization.Fortifications);
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.NotAllocated,
                    CommandSourceTable = enCommandSourceTable.NotCommand,
                    Coffers = new ParameterChanging<int?>(coffers, null),
                    Warriors = new ParameterChanging<int?>(warriors, null),
                    Defense = new ParameterChanging<double?>(null, exceptedDefense),
                    Descripton = $"НЕ РАСПРЕДЕЛЕНО:"
                }
            };
        }

        private IEnumerable<LineOfBudget> GetTotal(Domain organization, List<ICommand> organizationCommands)
        {
            var coffers = Lines.Sum(l => l.Coffers?.ExpectedValue);
            var investments = Lines.Sum(l => l.Investments?.ExpectedValue);
            var warriors = Lines.Sum(l => l.Warriors?.ExpectedValue);
            var defense = Lines.Sum(l => l.Defense?.ExpectedValue);
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.Total,
                    CommandSourceTable = enCommandSourceTable.NotCommand,
                    Coffers = new ParameterChanging<int?>(null, coffers),
                    Investments = new ParameterChanging<int?>(null, investments),
                    Warriors = new ParameterChanging<int?>(null, warriors),
                    Defense = new ParameterChanging<double?>(null, defense),
                    Descripton = $"ИТОГО: "
                }
            };
        }
    }
}
