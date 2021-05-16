using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Commands;
using YSI.CurseOfSilverCrown.Core.Interfaces;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using Microsoft.EntityFrameworkCore;

namespace YSI.CurseOfSilverCrown.Core.ViewModels
{
    public class Budget
    {
        private const int ExpectedLossesEvery = 10;

        public List<LineOfBudget> Lines { get; set; } = new List<LineOfBudget>();
        public OrganizationInfo Organization { get; private set; }


        public Budget(Domain organization, int initiatorId, ApplicationDbContext context)
        {
            var allCommand = GetAllCommandsAsync(organization, initiatorId, context).Result;
            Init(organization, allCommand);
        }

        public Budget(Domain organization, List<ICommand> organizationCommands)
        {
            Init(organization, organizationCommands);

        }

        private async Task<IEnumerable<ICommand>> GetAllCommandsAsync(Domain organization, int initiatorId, ApplicationDbContext context)
        {
            var allCommands = await context.Commands
                .Include(c => c.Domain)
                .Include(c => c.Target)
                .Include(c => c.Target2)
                .Where(c => c.DomainId == organization.Id &&
                    c.InitiatorDomainId == initiatorId)
                .Cast<ICommand>()
                .ToListAsync();

            var units = await context.Units
                .Include(c => c.Domain)
                .Include(c => c.Target)
                .Include(c => c.Target2)
                .Where(c => c.DomainId == organization.Id &&
                    c.InitiatorDomainId == initiatorId)
                .Cast<ICommand>()
                .ToListAsync();

            allCommands.AddRange(units);
            return allCommands;
        }

        private void Init(Domain organization, IEnumerable<ICommand> allCommand)
        {
            Lines = new List<LineOfBudget>();
            Organization = new OrganizationInfo(organization);
            var lineFunctions = new List<Func<Domain, List<ICommand>, IEnumerable<LineOfBudget>>>()
            {
                GetCurrent,

                WarSupportDefense,
                War,
                WarSupportAttack,
                GetGrowth,
                GetInvestments,
                GetFortifications,
                //GetBaseTax,
                GetAditionalTax,
                GetInvestmentProfit,
                VassalTax,
                GetSuzerainTax,
                GetIdleness,
                GetMaintenance,
                GetMaintenanceFortifications,
                GetGoldTransfers,
                VassalTransfers,
                Rebelion,

                GetNotAllocated,
                GetTotal
            };

            foreach (var func in lineFunctions)
                Lines.AddRange(func(organization, allCommand.ToList()));
        }

        private IEnumerable<LineOfBudget> GetCurrent(Domain organization, List<ICommand> organizationCommands)
        {
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.Current,
                    Coffers = organization.Coffers,
                    Warriors = organization.Warriors,
                    CoffersWillBe = organization.Coffers,
                    InvestmentsWillBe = organization.Investments,
                    WarriorsWillBe = organization.Warriors,
                    Descripton = "Имеется на начало сезона"
                }
            };
        }

        private IEnumerable<LineOfBudget> GetIdleness(Domain organization, List<ICommand> organizationCommands)
        {
            var command = organizationCommands.Single(c => c.TypeInt == (int)enCommandType.Idleness);
            return new [] { 
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.Idleness,
                    CoffersWillBe = -command.Coffers,
                    Descripton = "Затраты на содержание двора",
                    Editable = true,
                    CommandId = command.Id
                } 
            };
        }

        private IEnumerable<LineOfBudget> GetMaintenance(Domain organization, List<ICommand> organizationCommands)
        {
            var growth = organizationCommands.Single(c => c.TypeInt == (int)enCommandType.Growth);
            var currentWarriors = organization.Warriors;
            var newWarriors = growth.Coffers / WarriorParameters.Price;
            var expectedLosses = organizationCommands
                .Where(c => c.TypeInt == (int)enArmyCommandType.War || c.TypeInt == (int)enArmyCommandType.Rebellion)
                .Sum(w => w.Warriors / ExpectedLossesEvery);
            var expectedWarriorsForMaintenance = currentWarriors + newWarriors - expectedLosses;
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.Maintenance,
                    CoffersWillBe = -expectedWarriorsForMaintenance * WarriorParameters.Maintenance,
                    Descripton = "Ожидаемые затраты на содержание воинов"
                } 
            };
        }

        private IEnumerable<LineOfBudget> GetMaintenanceFortifications(Domain organization, List<ICommand> organizationCommands)
        {
            var newFortifications = organizationCommands.Single(c => c.TypeInt == (int)enCommandType.Fortifications).Coffers;
            var currentFortifications = organization.Fortifications;
            var expectedWarriorsForMaintenance = currentFortifications + newFortifications;
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.FortificationsMaintenance,
                    CoffersWillBe = -(int)Math.Round(expectedWarriorsForMaintenance * FortificationsParameters.MaintenancePercent),
                    Descripton = "Затраты на содержание укреплений"
                }
            };
        }

        private IEnumerable<LineOfBudget> GetGrowth(Domain organization, List<ICommand> organizationCommands)
        {
            var command = organizationCommands.Single(c => c.TypeInt == (int)enCommandType.Growth);
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.Growth,
                    Coffers = -command.Coffers,
                    CoffersWillBe = -command.Coffers,
                    WarriorsWillBe = (command.Coffers / WarriorParameters.Price),
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
                    Coffers = -command.Coffers,
                    CoffersWillBe = -command.Coffers,
                    InvestmentsWillBe = command.Coffers,
                    Descripton = "Вложения средств в экономику провинции",
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
                    Coffers = -command.Coffers,
                    CoffersWillBe = -command.Coffers,
                    FortificationsWillBe = command.Coffers,
                    Descripton = "Вложения средств в постройку укреплений",
                    Editable = true,
                    CommandId = command.Id
                }
            };
        }

        private IEnumerable<LineOfBudget> GetInvestmentProfit(Domain organization, List<ICommand> organizationCommands)
        {
            var investments = organizationCommands.Single(c => c.TypeInt == (int)enCommandType.Investments);
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.InvestmentProfit,
                    CoffersWillBe = Constants.MinTax + InvestmentsHelper.GetInvestmentTax(organization.Investments + investments.Coffers),
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
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.AditionalTax,
                    Warriors = -additoinalWarriors,
                    CoffersWillBe = Constants.GetAdditionalTax(additoinalWarriors, 0.5),
                    DefenseWillBe = additoinalWarriors *
                        FortificationsHelper.GetWariorDefenseCoeficient(WarConstants.WariorDefenseTax, organization.Fortifications),
                    Descripton = "Дополнительный сбор налогов",
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
                CoffersWillBe = (int)Math.Round(Constants.MinTax * Constants.BaseVassalTax),
                Descripton = $"Получение налогов от вассала {vassal.Name}"
            });
        }

        private IEnumerable<LineOfBudget> GetSuzerainTax(Domain organization, List<ICommand> organizationCommands)
        {
            if (organization.Suzerain == null)
                return Array.Empty<LineOfBudget>();

            var additoinalWarriors = organizationCommands.SingleOrDefault(c => c.TypeInt == (int)enArmyCommandType.CollectTax)?.Warriors ?? 0;
            var investments = organizationCommands.Single(c => c.TypeInt == (int)enCommandType.Investments);
            var allIncome = Constants.MinTax +
                Constants.GetAdditionalTax(additoinalWarriors, 0.5) +
                InvestmentsHelper.GetInvestmentTax(organization.Investments + investments.Coffers);

            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.SuzerainTax,
                    CoffersWillBe = (int)(-Math.Round(allIncome * Constants.BaseVassalTax)),
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
                Warriors = -command.Warriors,
                WarriorsWillBe = -command.Warriors / ExpectedLossesEvery,
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
                Warriors = -command.Warriors,
                WarriorsWillBe = -command.Warriors / ExpectedLossesEvery,
                Descripton = $"Помощь провинции {command.Target2?.Name} в нападении на {command.Target?.Name}",
                Editable = true,
                Deleteable = true,
                CommandId = command.Id
            });
        }

        private IEnumerable<LineOfBudget> WarSupportDefense(Domain organization, List<ICommand> organizationCommands)
        {
            var commands = organizationCommands.Where(c => c.TypeInt == (int)enArmyCommandType.WarSupportDefense);
            return commands.Select(command => new LineOfBudget
            {
                Type = enLineOfBudgetType.WarSupportDefense,
                Warriors = -command.Warriors,
                DefenseWillBe = command.TargetDomainId == command.DomainId
                        ? command.Warriors * 
                            FortificationsHelper.GetWariorDefenseCoeficient(WarConstants.WariorDefenseSupport, organization.Fortifications)
                        : null,
                Descripton = $"Защита провинции {command.Target?.Name}",
                Editable = true,
                Deleteable = true,
                CommandId = command.Id
            });
        }

        private IEnumerable<LineOfBudget> VassalTransfers(Domain organization, List<ICommand> organizationCommands)
        {
            var commands = organizationCommands.Where(c => c.TypeInt == (int)enCommandType.VassalTransfer);
            return commands.Select(command => new LineOfBudget
            {
                Type = enLineOfBudgetType.VassalTransfer,
                Descripton = command.TargetDomainId == command.Target2DomainId
                    ? $"Освобождение провинции {command.Target.Name} от вассальной клятвы"
                    : command.DomainId == command.TargetDomainId
                        ? $"Добровольная присяга провиции {command.Target2.Name}"
                        : $"Передача провинции {command.Target.Name} под покровительство провинции {command.Target2.Name}",
                Editable = true,
                Deleteable = true,
                CommandId = command.Id
            });
        }

        private IEnumerable<LineOfBudget> Rebelion(Domain organization, List<ICommand> organizationCommands)
        {
            var command = organizationCommands.SingleOrDefault(c => c.TypeInt == (int)enArmyCommandType.Rebellion);
            if (command == null)
                return new LineOfBudget[0];

            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.Rebelion,
                    Warriors = -command.Warriors,
                    WarriorsWillBe = -command.Warriors / 10,
                    Descripton = "Востание против сюзерена",
                    Editable = organization.SuzerainId != null,
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
                Descripton = $"Передача золота в провинцию {command.Target.Name}",
                Coffers = -command.Coffers,
                CoffersWillBe = -command.Coffers,
                Editable = true,
                Deleteable = true,
                CommandId = command.Id
            });
        }

        private IEnumerable<LineOfBudget> GetNotAllocated(Domain organization, List<ICommand> organizationCommands)
        {
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.NotAllocated,
                    Coffers = Lines.Sum(l => l.Coffers),
                    Warriors = Lines.Sum(l => l.Warriors),
                    DefenseWillBe = Lines.Sum(l => l.Warriors) * 
                        FortificationsHelper.GetWariorDefenseCoeficient(WarConstants.WariorDefenseTax, organization.Fortifications),
                    Descripton = $"НЕ РАСПРЕДЕЛЕНО:"
                }
            };
        }

        private IEnumerable<LineOfBudget> GetTotal(Domain organization, List<ICommand> organizationCommands)
        {
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.Total,
                    CoffersWillBe = Lines.Sum(l => l.CoffersWillBe),
                    InvestmentsWillBe = Lines.Sum(l => l.InvestmentsWillBe),
                    WarriorsWillBe = Lines.Sum(l => l.WarriorsWillBe),
                    DefenseWillBe = Lines.Sum(l => l.DefenseWillBe),
                    Descripton = $"ИТОГО: "
                }
            };
        }
    }

    public class LineOfBudget
    {
        public enLineOfBudgetType Type { get; set; }
        public string Descripton { get; set; }
        public int? Coffers { get; set; }
        public int? Warriors { get; set; }
        public int? CoffersWillBe { get; set; }
        public int? InvestmentsWillBe { get; set; }
        public int? FortificationsWillBe { get; set; }
        public int? WarriorsWillBe { get; set; }
        public double? DefenseWillBe { get; set; }
        public bool Editable { get; set; }
        public bool Deleteable { get; set; }

        public int CommandId { get; set; }
    }

    public enum enLineOfBudgetType
    {
        Current = 0,

        Idleness = 1,
        Maintenance = 2,
        Growth = 3,
        BaseTax = 4,
        VassalTax = 5,
        SuzerainTax = 6,
        War = 7,
        Investments = 8,
        WarSupportDefense = 9,
        InvestmentProfit = 10,
        AditionalTax = 11,
        Fortifications = 12,
        FortificationsMaintenance = 13,
        GoldTransfer = 14,
        Rebelion = 15,
        WarSupportAtack = 16,

        VassalTransfer = 70,

        NotAllocated = 90,
        Total = 100
    }
}
