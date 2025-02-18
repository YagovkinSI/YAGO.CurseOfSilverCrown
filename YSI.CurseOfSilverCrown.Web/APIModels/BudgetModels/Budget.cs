using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Database;
using YSI.CurseOfSilverCrown.Web.Database.Commands;
using YSI.CurseOfSilverCrown.Web.Database.Domains;
using YSI.CurseOfSilverCrown.Web.Database.Units;
using YSI.CurseOfSilverCrown.Web.Helpers;
using YSI.CurseOfSilverCrown.Web.Parameters;

namespace YSI.CurseOfSilverCrown.Web.APIModels.BudgetModels
{
    public class Budget
    {
        private const int ExpectedLossesEvery = 10;

        public List<BudgetLine> Lines { get; set; } = new List<BudgetLine>();
        public Domain Organization { get; private set; }

        private List<Func<Domain, List<ICommand>, IEnumerable<BudgetLine>>> LineFunctions => new()
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

        public Budget(ApplicationDbContext context, Domain organization)
        {
            var allCommand = GetAllCommandsAsync(organization, context).Result;
            Init(organization, allCommand);
        }

        private async Task<IEnumerable<ICommand>> GetAllCommandsAsync(Domain organization, ApplicationDbContext context)
        {
            var allCommands = await context.Commands
                .Where(c => c.DomainId == organization.Id)
                .Cast<ICommand>()
                .ToListAsync();

            var units = await context.Units
                .Where(c => c.DomainId == organization.Id)
                .Cast<ICommand>()
                .ToListAsync();

            allCommands.AddRange(units);
            return allCommands;
        }

        private void Init(Domain domain, IEnumerable<ICommand> allCommand)
        {
            Lines = new List<BudgetLine>();
            Organization = domain;
            foreach (var func in LineFunctions)
                Lines.AddRange(func(domain, allCommand.ToList()));
        }

        private IEnumerable<BudgetLine> GetCurrent(Domain domain, List<ICommand> organizationCommands)
        {
            var currentWarriors = domain.WarriorCount;
            return new[] {
                new BudgetLine
                {
                    Type = BudgetLineType.Current,
                    CommandSourceTable = BudgetLineSource.NotCommand,
                    Coffers = new ParameterChanging<int?>(domain.Gold, domain.Gold),
                    Warriors = new ParameterChanging<int?>(currentWarriors, currentWarriors),
                    Investments = new ParameterChanging<int?>(domain.InvestmentsShowed, domain.InvestmentsShowed),
                    Descripton = "Имеется на начало сезона"
                }
            };
        }

        private IEnumerable<BudgetLine> GetMaintenance(Domain organization, List<ICommand> organizationCommands)
        {
            var growth = organizationCommands.Single(c => c.TypeInt == (int)CommandType.Growth);
            var currentWarriors = organization.WarriorCount;
            var newWarriors = growth.Gold / WarriorParameters.Price;
            var expectedLosses = organizationCommands
                .Where(c => c.TypeInt == (int)UnitCommandType.War || c.TypeInt == (int)UnitCommandType.WarSupportAttack)
                .Sum(w => w.Warriors / ExpectedLossesEvery);
            var expectedWarriorsForMaintenance = currentWarriors + newWarriors - expectedLosses;
            var expectedCoffers = -expectedWarriorsForMaintenance * WarriorParameters.Maintenance;
            return new[] {
                new BudgetLine
                {
                    Type = BudgetLineType.Maintenance,
                    CommandSourceTable = BudgetLineSource.NotCommand,
                    Coffers = new ParameterChanging<int?>(null, expectedCoffers),
                    Descripton = "Ожидаемые затраты на содержание воинов"
                }
            };
        }

        private IEnumerable<BudgetLine> GetMaintenanceFortifications(Domain organization, List<ICommand> organizationCommands)
        {
            var newFortifications = organizationCommands.Single(c => c.TypeInt == (int)CommandType.Fortifications).Gold;
            var currentFortifications = organization.Fortifications;
            var expectedWarriorsForMaintenance = currentFortifications + newFortifications;
            var expectedCoffers = -(int)Math.Round(expectedWarriorsForMaintenance * FortificationsParameters.MaintenancePercent);
            return new[] {
                new BudgetLine
                {
                    Type = BudgetLineType.FortificationsMaintenance,
                    CommandSourceTable = BudgetLineSource.NotCommand,
                    Coffers = new ParameterChanging<int?>(null, expectedCoffers),
                    Descripton = "Затраты на содержание укреплений"
                }
            };
        }

        private IEnumerable<BudgetLine> GetGrowth(Domain organization, List<ICommand> organizationCommands)
        {
            var command = organizationCommands.Single(c => c.TypeInt == (int)CommandType.Growth);
            var expectedWarriorios = command.Gold / WarriorParameters.Price;
            return new[] {
                new BudgetLine
                {
                    Type = BudgetLineType.Growth,
                    CommandSourceTable = BudgetLineSource.Commands,
                    Coffers = new ParameterChanging<int?>(-command.Gold, -command.Gold),
                    Warriors = new ParameterChanging<int?>(null, expectedWarriorios),
                    Descripton = "Затраты на набор новых воинов",
                    Editable = true,
                    CommandId = command.Id
                }
            };
        }

        private IEnumerable<BudgetLine> GetInvestments(Domain organization, List<ICommand> organizationCommands)
        {
            var command = organizationCommands.Single(c => c.TypeInt == (int)CommandType.Investments);
            return new[] {
                new BudgetLine
                {
                    Type = BudgetLineType.Investments,
                    CommandSourceTable = BudgetLineSource.Commands,
                    Coffers = new ParameterChanging<int?>(-command.Gold, -command.Gold),
                    Investments = new ParameterChanging<int?>(null, command.Gold),
                    Descripton = "Вложения средств в имущество владения",
                    Editable = true,
                    CommandId = command.Id
                }
            };
        }

        private IEnumerable<BudgetLine> GetFortifications(Domain organization, List<ICommand> organizationCommands)
        {
            var command = organizationCommands.Single(c => c.TypeInt == (int)CommandType.Fortifications);
            return new[] {
                new BudgetLine
                {
                    Type = BudgetLineType.Fortifications,
                    CommandSourceTable = BudgetLineSource.Commands,
                    Coffers = new ParameterChanging<int?>(-command.Gold, -command.Gold),
                    Fortifications = new ParameterChanging<int?>(null, command.Gold),
                    Descripton = "Вложения средств в постройку укреплений",
                    Editable = true,
                    CommandId = command.Id
                }
            };
        }

        private IEnumerable<BudgetLine> GetInvestmentProfit(Domain organization, List<ICommand> organizationCommands)
        {
            var investments = organizationCommands.Single(c => c.TypeInt == (int)CommandType.Investments);
            var expectedCoffers = InvestmentsHelper.GetInvestmentTax(organization.Investments + investments.Gold);
            return new[] {
                new BudgetLine
                {
                    Type = BudgetLineType.InvestmentProfit,
                    CommandSourceTable = BudgetLineSource.Commands,
                    Coffers = new ParameterChanging<int?>(null, expectedCoffers),
                    Descripton = "Основной налог"
                }
            };
        }

        private IEnumerable<BudgetLine> GetAditionalTax(Domain organization, List<ICommand> organizationCommands)
        {
            var command = organizationCommands.SingleOrDefault(c => c.TypeInt == (int)UnitCommandType.CollectTax);
            if (command == null)
                return new BudgetLine[0];

            var additoinalWarriors = command.Warriors;
            var expectedCoffers = Constants.GetAdditionalTax(additoinalWarriors);
            return new[] {
                new BudgetLine
                {
                    Type = BudgetLineType.AditionalTax,
                    CommandSourceTable = BudgetLineSource.Units,
                    Warriors = new ParameterChanging<int?>(-additoinalWarriors, null),
                    Coffers = new ParameterChanging<int?>(null, expectedCoffers),
                    Descripton = "Временный роспуск отряда",
                    Editable = true,
                    CommandId = command.Id
                }
            };
        }

        private IEnumerable<BudgetLine> VassalTax(Domain organization, List<ICommand> organizationCommands)
        {
            var vassals = organization.Vassals;
            return vassals.Select(vassal => new BudgetLine
            {
                Type = BudgetLineType.VassalTax,
                CommandSourceTable = BudgetLineSource.NotCommand,
                Coffers = new ParameterChanging<int?>
                (
                    null,
                    (int)Math.Round(InvestmentsHelper.GetInvestmentTax(vassal.Investments) * Constants.BaseVassalTax)
                ),
                Descripton = $"Получение налогов от вассала {vassal.Name}"
            });
        }

        private IEnumerable<BudgetLine> GetSuzerainTax(Domain organization, List<ICommand> organizationCommands)
        {
            if (organization.Suzerain == null)
                return Array.Empty<BudgetLine>();

            var additoinalWarriors = organizationCommands.SingleOrDefault(c => c.TypeInt == (int)UnitCommandType.CollectTax)?.Warriors ?? 0;
            var investments = organizationCommands.Single(c => c.TypeInt == (int)CommandType.Investments);
            var allIncome = Constants.GetAdditionalTax(additoinalWarriors) +
                InvestmentsHelper.GetInvestmentTax(organization.Investments + investments.Gold);
            var expectedCoffers = (int)-Math.Round(allIncome * Constants.BaseVassalTax);

            return new[] {
                new BudgetLine
                {
                    Type = BudgetLineType.SuzerainTax,
                    CommandSourceTable = BudgetLineSource.NotCommand,
                    Coffers = new ParameterChanging<int?>(null, expectedCoffers),
                    Descripton = $"Передача налога сюзерену в {organization.Suzerain.Name}"
                }
            };
        }

        private IEnumerable<BudgetLine> War(Domain organization, List<ICommand> organizationCommands)
        {
            var commands = organizationCommands.Where(c => c.TypeInt == (int)UnitCommandType.War);
            return commands.Select(command => new BudgetLine
            {
                Type = BudgetLineType.War,
                CommandSourceTable = BudgetLineSource.Units,
                Warriors = new ParameterChanging<int?>(-command.Warriors, -command.Warriors / ExpectedLossesEvery),
                Descripton = $"Нападение на {command.Target?.Name}",
                Editable = true,
                Deleteable = true,
                CommandId = command.Id
            });
        }

        private IEnumerable<BudgetLine> WarSupportAttack(Domain organization, List<ICommand> organizationCommands)
        {
            var commands = organizationCommands.Where(c => c.TypeInt == (int)UnitCommandType.WarSupportAttack);
            return commands.Select(command => new BudgetLine
            {
                Type = BudgetLineType.WarSupportAtack,
                CommandSourceTable = BudgetLineSource.Units,
                Warriors = new ParameterChanging<int?>(-command.Warriors, -command.Warriors / ExpectedLossesEvery),
                Descripton = $"Помощь владению {command.Target2?.Name} в нападении на {command.Target?.Name}",
                Editable = true,
                Deleteable = true,
                CommandId = command.Id
            });
        }

        private IEnumerable<BudgetLine> WarSupportDefense(Domain organization, List<ICommand> organizationCommands)
        {
            var commands = organizationCommands.Where(c => c.TypeInt == (int)UnitCommandType.WarSupportDefense);
            var lines = new List<BudgetLine>();
            foreach (var command in commands)
            {
                var line = new BudgetLine
                {
                    Type = BudgetLineType.WarSupportDefense,
                    CommandSourceTable = BudgetLineSource.Units,
                    Warriors = new ParameterChanging<int?>(-command.Warriors, null),
                    Descripton = $"Защита владения {command.Target?.Name}",
                    Editable = true,
                    Deleteable = true,
                    CommandId = command.Id
                };
                lines.Add(line);
            }
            return lines;
        }

        private IEnumerable<BudgetLine> VassalTransfers(Domain organization, List<ICommand> organizationCommands)
        {
            var commands = organizationCommands.Where(c => c.TypeInt == (int)CommandType.VassalTransfer);
            return commands.Select(command => new BudgetLine
            {
                Type = BudgetLineType.VassalTransfer,
                CommandSourceTable = BudgetLineSource.Commands,
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

        private IEnumerable<BudgetLine> Rebelion(Domain organization, List<ICommand> organizationCommands)
        {
            var command = organizationCommands.SingleOrDefault(c => c.TypeInt == (int)CommandType.Rebellion);
            if (command == null)
                return new BudgetLine[0];

            return new[] {
                new BudgetLine
                {
                    Type = BudgetLineType.Rebelion,
                    CommandSourceTable = BudgetLineSource.Commands,
                    Descripton = "Объявление независимости",
                    Editable = false,
                    Deleteable = true,
                    CommandId = command.Id
                }
            };
        }

        private IEnumerable<BudgetLine> GetGoldTransfers(Domain organization, List<ICommand> organizationCommands)
        {
            var commands = organizationCommands.Where(c => c.TypeInt == (int)CommandType.GoldTransfer);
            return commands.Select(command => new BudgetLine
            {
                Type = BudgetLineType.GoldTransfer,
                CommandSourceTable = BudgetLineSource.Commands,
                Descripton = $"Передача золота во владение {command.Target.Name}",
                Coffers = new ParameterChanging<int?>(-command.Gold, -command.Gold),
                Editable = true,
                Deleteable = true,
                CommandId = command.Id
            });
        }

        private IEnumerable<BudgetLine> GetNotAllocated(Domain organization, List<ICommand> organizationCommands)
        {
            var coffers = Lines.Sum(l => l.Coffers?.CurrentValue);
            var warriors = Lines.Sum(l => l.Warriors?.CurrentValue);
            return new[] {
                new BudgetLine
                {
                    Type = BudgetLineType.NotAllocated,
                    CommandSourceTable = BudgetLineSource.NotCommand,
                    Coffers = new ParameterChanging<int?>(coffers, null),
                    Warriors = new ParameterChanging<int?>(warriors, null),
                    Descripton = $"НЕ РАСПРЕДЕЛЕНО:"
                }
            };
        }

        private IEnumerable<BudgetLine> GetTotal(Domain organization, List<ICommand> organizationCommands)
        {
            var coffers = Lines.Sum(l => l.Coffers?.ExpectedValue);
            var investments = Lines.Sum(l => l.Investments?.ExpectedValue);
            var warriors = Lines.Sum(l => l.Warriors?.ExpectedValue);
            return new[] {
                new BudgetLine
                {
                    Type = BudgetLineType.Total,
                    CommandSourceTable = BudgetLineSource.NotCommand,
                    Coffers = new ParameterChanging<int?>(null, coffers),
                    Investments = new ParameterChanging<int?>(null, investments),
                    Warriors = new ParameterChanging<int?>(null, warriors),
                    Descripton = $"ИТОГО: "
                }
            };
        }
    }
}
