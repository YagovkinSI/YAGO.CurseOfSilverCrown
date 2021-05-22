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
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.BL.Models.Main;
using YSI.CurseOfSilverCrown.Core.BL.Models.Min;

namespace YSI.CurseOfSilverCrown.Core.ViewModels
{
    public class Budget
    {
        private const int ExpectedLossesEvery = 10;

        public List<LineOfBudget> Lines { get; set; } = new List<LineOfBudget>();
        public DomainMin Organization { get; private set; }
        public ApplicationDbContext Context { get; }


        public Budget(ApplicationDbContext context, DomainMain organization, int initiatorId)
        {
            Context = context;
            var allCommand = GetAllCommandsAsync(organization, initiatorId, context).Result;
            Init(organization, allCommand);
        }

        public Budget(ApplicationDbContext context, DomainMain organization, List<ICommand> organizationCommands)
        {
            Context = context;
            Init(organization, organizationCommands);
        }

        private async Task<IEnumerable<ICommand>> GetAllCommandsAsync(DomainMin organization, int initiatorId, ApplicationDbContext context)
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

        private void Init(DomainMain domain, IEnumerable<ICommand> allCommand)
        {
            Lines = new List<LineOfBudget>();
            Organization = domain;
            var lineFunctions = new List<Func<DomainMain, List<ICommand>, IEnumerable<LineOfBudget>>>()
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
                Lines.AddRange(func(domain, allCommand.ToList()));
        }

        private IEnumerable<LineOfBudget> GetCurrent(DomainMain domain, List<ICommand> organizationCommands)
        {
            var currentWarriors = domain.Warriors;
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.Current,
                    Coffers = domain.Coffers,
                    Warriors = currentWarriors,
                    CoffersWillBe = domain.Coffers,
                    InvestmentsWillBe = domain.Investments,
                    WarriorsWillBe = currentWarriors,
                    Descripton = "Имеется на начало сезона"
                }
            };
        }

        private IEnumerable<LineOfBudget> GetIdleness(DomainMain organization, List<ICommand> organizationCommands)
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

        private IEnumerable<LineOfBudget> GetMaintenance(DomainMain organization, List<ICommand> organizationCommands)
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

        private IEnumerable<LineOfBudget> GetMaintenanceFortifications(DomainMain organization, List<ICommand> organizationCommands)
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

        private IEnumerable<LineOfBudget> GetGrowth(DomainMain organization, List<ICommand> organizationCommands)
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

        private IEnumerable<LineOfBudget> GetInvestments(DomainMain organization, List<ICommand> organizationCommands)
        {
            var command = organizationCommands.Single(c => c.TypeInt == (int)enCommandType.Investments);
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.Investments,
                    Coffers = -command.Coffers,
                    CoffersWillBe = -command.Coffers,
                    InvestmentsWillBe = command.Coffers,
                    Descripton = "Вложения средств в имущество владения",
                    Editable = true,
                    CommandId = command.Id
                }
            };
        }

        private IEnumerable<LineOfBudget> GetFortifications(DomainMain organization, List<ICommand> organizationCommands)
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

        private IEnumerable<LineOfBudget> GetInvestmentProfit(DomainMain organization, List<ICommand> organizationCommands)
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

        private IEnumerable<LineOfBudget> GetAditionalTax(DomainMain organization, List<ICommand> organizationCommands)
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

        private IEnumerable<LineOfBudget> VassalTax(DomainMain organization, List<ICommand> organizationCommands)
        {
            var vassals = organization.Vassals;
            return vassals.Select(vassal => new LineOfBudget
            {
                Type = enLineOfBudgetType.VassalTax,
                CoffersWillBe = (int)Math.Round(Constants.MinTax * Constants.BaseVassalTax),
                Descripton = $"Получение налогов от вассала {vassal.Name}"
            });
        }

        private IEnumerable<LineOfBudget> GetSuzerainTax(DomainMain organization, List<ICommand> organizationCommands)
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

        private IEnumerable<LineOfBudget> War(DomainMain organization, List<ICommand> organizationCommands)
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

        private IEnumerable<LineOfBudget> WarSupportAttack(DomainMain organization, List<ICommand> organizationCommands)
        {
            var commands = organizationCommands.Where(c => c.TypeInt == (int)enArmyCommandType.WarSupportAttack);
            return commands.Select(command => new LineOfBudget
            {
                Type = enLineOfBudgetType.WarSupportAtack,
                Warriors = -command.Warriors,
                WarriorsWillBe = -command.Warriors / ExpectedLossesEvery,
                Descripton = $"Помощь владению {command.Target2?.Name} в нападении на {command.Target?.Name}",
                Editable = true,
                Deleteable = true,
                CommandId = command.Id
            });
        }

        private IEnumerable<LineOfBudget> WarSupportDefense(DomainMain organization, List<ICommand> organizationCommands)
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
                Descripton = $"Защита владения {command.Target?.Name}",
                Editable = true,
                Deleteable = true,
                CommandId = command.Id
            });
        }

        private IEnumerable<LineOfBudget> VassalTransfers(DomainMain organization, List<ICommand> organizationCommands)
        {
            var commands = organizationCommands.Where(c => c.TypeInt == (int)enCommandType.VassalTransfer);
            return commands.Select(command => new LineOfBudget
            {
                Type = enLineOfBudgetType.VassalTransfer,
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

        private IEnumerable<LineOfBudget> Rebelion(DomainMain organization, List<ICommand> organizationCommands)
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

        private IEnumerable<LineOfBudget> GetGoldTransfers(DomainMain organization, List<ICommand> organizationCommands)
        {
            var commands = organizationCommands.Where(c => c.TypeInt == (int)enCommandType.GoldTransfer);
            return commands.Select(command => new LineOfBudget
            {
                Type = enLineOfBudgetType.GoldTransfer,
                Descripton = $"Передача золота во владение {command.Target.Name}",
                Coffers = -command.Coffers,
                CoffersWillBe = -command.Coffers,
                Editable = true,
                Deleteable = true,
                CommandId = command.Id
            });
        }

        private IEnumerable<LineOfBudget> GetNotAllocated(DomainMain organization, List<ICommand> organizationCommands)
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

        private IEnumerable<LineOfBudget> GetTotal(DomainMain organization, List<ICommand> organizationCommands)
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
