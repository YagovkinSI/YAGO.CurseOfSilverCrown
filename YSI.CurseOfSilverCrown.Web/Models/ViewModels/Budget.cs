using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.Web.Models.ViewModels
{
    public class Budget
    {
        private const int ExpectedLossesEvery = 10;

        public List<LineOfBudget> Lines { get; set; } = new List<LineOfBudget>();

        public Budget(Organization organization, List<Command> organizationCommands)
        {
            Lines = new List<LineOfBudget>();
            var lineFunctions = new List<Func<Organization, List<Command>, IEnumerable<LineOfBudget>>>()
            {
                GetCurrent,

                WarSupportDefense,
                War,
                GetGrowth,
                GetInvestments,
                GetBaseTax,
                GetAditionalTax,
                GetInvestmentProfit,
                VassalTax,
                GetSuzerainTax,
                GetIdleness,
                GetMaintenance,

                GetNotAllocated,
                GetTotal
            };

            foreach (var func in lineFunctions)
                Lines.AddRange(func(organization, organizationCommands));
        }

        private IEnumerable<LineOfBudget> GetCurrent(Organization organization, List<Command> organizationCommands)
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

        private IEnumerable<LineOfBudget> GetIdleness(Organization organization, List<Command> organizationCommands)
        {
            var command = organizationCommands.Single(c => c.Type == enCommandType.Idleness);
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

        private IEnumerable<LineOfBudget> GetMaintenance(Organization organization, List<Command> organizationCommands)
        {
            var growth = organizationCommands.Single(c => c.Type == enCommandType.Growth);
            var currentWarriors = organization.Warriors;
            var newWarriors = growth.Coffers / Constants.OutfitWarrioir;
            var expectedLosses = organizationCommands
                .Where(c => c.Type == enCommandType.War)
                .Sum(w => w.Warriors / ExpectedLossesEvery);
            var expectedWarriorsForMaintenance = currentWarriors + newWarriors - expectedLosses;
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.Maintenance,
                    CoffersWillBe = -expectedWarriorsForMaintenance * Constants.MaintenanceWarrioir,
                    Descripton = "Ожидаемые затраты на содержание воинов"
                } 
            };
        }

        private IEnumerable<LineOfBudget> GetGrowth(Organization organization, List<Command> organizationCommands)
        {
            var command = organizationCommands.Single(c => c.Type == enCommandType.Growth);
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.Growth,
                    Coffers = -command.Coffers,
                    CoffersWillBe = -command.Coffers,
                    WarriorsWillBe = (command.Coffers / Constants.OutfitWarrioir),
                    Descripton = "Затраты на набор новых воинов",
                    Editable = true,
                    CommandId = command.Id
                }
            };
        }

        private IEnumerable<LineOfBudget> GetInvestments(Organization organization, List<Command> organizationCommands)
        {
            var command = organizationCommands.Single(c => c.Type == enCommandType.Investments);
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

        private IEnumerable<LineOfBudget> GetBaseTax(Organization organization, List<Command> organizationCommands)
        {
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.BaseTax,
                    CoffersWillBe = Constants.MinTax,
                    Descripton = "Контроль провинции"
                }
            };
        }

        private IEnumerable<LineOfBudget> GetAditionalTax(Organization organization, List<Command> organizationCommands)
        {
            var command = organizationCommands.Single(c => c.Type == enCommandType.CollectTax);
            var additoinalWarriors = command.Warriors;
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.AditionalTax,
                    Warriors = -additoinalWarriors,
                    CoffersWillBe = Constants.GetAdditionalTax(additoinalWarriors, 0.5),
                    DefenseWillBe = additoinalWarriors * WarConstants.WariorDefenseTax,
                    Descripton = "Дополнительный сбор налогов",
                    Editable = true,
                    CommandId = command.Id
                }
            };
        }

        private IEnumerable<LineOfBudget> GetInvestmentProfit(Organization organization, List<Command> organizationCommands)
        {
            var investments = organizationCommands.Single(c => c.Type == enCommandType.Investments);
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.InvestmentProfit,
                    CoffersWillBe = Constants.GetInvestmentTax(organization.Investments + investments.Coffers),
                    Descripton = "Доход от инвестиций"
                }
            };
        }

        private IEnumerable<LineOfBudget> VassalTax(Organization organization, List<Command> organizationCommands)
        {
            var vassals = organization.Vassals;
            return vassals.Select(vassal => new LineOfBudget
            {
                Type = enLineOfBudgetType.VassalTax,
                CoffersWillBe = (int)Math.Round(Constants.MinTax * Constants.BaseVassalTax),
                Descripton = $"Получение налогов от вассала {vassal.Name}"
            });
        }

        private IEnumerable<LineOfBudget> GetSuzerainTax(Organization organization, List<Command> organizationCommands)
        {
            if (organization.Suzerain == null)
                return Array.Empty<LineOfBudget>();

            var additoinalWarriors = organizationCommands.Single(c => c.Type == enCommandType.CollectTax).Warriors;
            var investments = organizationCommands.Single(c => c.Type == enCommandType.Investments);
            var allIncome = Constants.MinTax +
                Constants.GetAdditionalTax(additoinalWarriors, 0.5) +
                Constants.GetInvestmentTax(organization.Investments + investments.Coffers);

            var command = organizationCommands.Single(c => c.Type == enCommandType.CollectTax);

            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.SuzerainTax,
                    CoffersWillBe = (int)(-Math.Round(allIncome * Constants.BaseVassalTax)),
                    Descripton = $"Передача налога сюзерену в {organization.Suzerain.Name}"
                }
            };
        }

        private IEnumerable<LineOfBudget> War(Organization organization, List<Command> organizationCommands)
        {
            var commands = organizationCommands.Where(c => c.Type == enCommandType.War);
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

        private IEnumerable<LineOfBudget> WarSupportDefense(Organization organization, List<Command> organizationCommands)
        {
            var commands = organizationCommands.Where(c => c.Type == enCommandType.WarSupportDefense);
            return commands.Select(command => new LineOfBudget
            {
                Type = enLineOfBudgetType.WarSupportDefense,
                Warriors = -command.Warriors,
                DefenseWillBe = command.TargetOrganizationId == command.OrganizationId
                        ? command.Warriors * WarConstants.WariorDefenseSupport
                        : null,
                Descripton = $"Защита провинции {command.Target?.Name}",
                Editable = true,
                Deleteable = true,
                CommandId = command.Id
            });
        }

        private IEnumerable<LineOfBudget> GetNotAllocated(Organization organization, List<Command> organizationCommands)
        {
            return new[] {
                new LineOfBudget
                {
                    Type = enLineOfBudgetType.NotAllocated,
                    Coffers = Lines.Sum(l => l.Coffers),
                    Warriors = Lines.Sum(l => l.Warriors),
                    DefenseWillBe = Lines.Sum(l => l.Warriors) * WarConstants.WariorDefenseTax,
                    Descripton = $"НЕ РАСПРЕДЕЛЕНО:"
                }
            };
        }

        private IEnumerable<LineOfBudget> GetTotal(Organization organization, List<Command> organizationCommands)
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
        public int? WarriorsWillBe { get; set; }
        public double? DefenseWillBe { get; set; }
        public bool Editable { get; set; }
        public bool Deleteable { get; set; }

        public string CommandId { get; set; }
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


        NotAllocated = 90,
        Total = 100
    }
}
