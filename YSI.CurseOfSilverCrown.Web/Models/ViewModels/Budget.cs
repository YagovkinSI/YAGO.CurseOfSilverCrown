using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.Models.ViewModels
{
    public class Budget
    {
        public List<LineOfBudget> Lines { get; set; } = new List<LineOfBudget>();

        public Budget(Organization organization, List<Command> organizationCommands)
        {
            Lines = new List<LineOfBudget>();

            var line = new LineOfBudget
            {
                Type = enLineOfBudgetType.Current,
                Coffers = organization.Coffers,
                Warriors = organization.Warriors,
                CoffersWillBe = organization.Coffers,
                InvestmentsWillBe = organization.Investments,
                WarriorsWillBe = organization.Warriors,
                Descripton = "Имеется на начало сезона"
            };
            Lines.Add(line);

            var command = organizationCommands.Single(c => c.Type == Web.Enums.enCommandType.Idleness);
            line = new LineOfBudget
            {
                Type = enLineOfBudgetType.Idleness,
                CoffersWillBe = -command.Coffers,
                Descripton = "Затраты на содержание двора",
                Editable = true,
                CommandId = command.Id
            };
            Lines.Add(line);

            var growth = organizationCommands.Single(c => c.Type == Web.Enums.enCommandType.Growth);
            line = new LineOfBudget
            {
                Type = enLineOfBudgetType.Maintenance,
                CoffersWillBe = -(organization.Warriors + growth.Coffers / Constants.OutfitWarrioir) * Constants.MaintenanceWarrioir,
                Descripton = "Затраты на содержание воинов (включая новобранцев)"
            };
            Lines.Add(line);

            command = growth;
            line = new LineOfBudget
            {
                Type = enLineOfBudgetType.Growth,
                Coffers = -command.Coffers,
                CoffersWillBe = -command.Coffers,
                WarriorsWillBe = (command.Coffers / Constants.OutfitWarrioir),
                Descripton = "Затраты на набор новых воинов",
                Editable = true,
                CommandId = command.Id
            };
            Lines.Add(line);

            command = organizationCommands.Single(c => c.Type == Web.Enums.enCommandType.Investments);
            line = new LineOfBudget
            {
                Type = enLineOfBudgetType.Investments,
                Coffers = -command.Coffers,
                InvestmentsWillBe = command.Coffers,
                Descripton = "Вложения средств в экономику провинции",
                Editable = true,
                CommandId = command.Id
            };
            Lines.Add(line);

            command = organizationCommands.Single(c => c.Type == Web.Enums.enCommandType.CollectTax);
            line = new LineOfBudget
            {
                Type = enLineOfBudgetType.Tax,
                Warriors = -command.Warriors,
                CoffersWillBe = TaxAction.GetTax(command.Warriors, organization.Investments, 0.5),
                Descripton = "Сбор налогов с земель провинции",
                Editable = true,
                CommandId = command.Id
            };
            Lines.Add(line);

            var vassals = organization.Vassals;
            foreach (var vassal in vassals)
            {
                line = new LineOfBudget
                {
                    Type = enLineOfBudgetType.VassalTax,
                    CoffersWillBe = Constants.VassalTax,
                    Descripton = $"Получение налогов от вассала {vassal.Name}"
                };
                Lines.Add(line);
            }

            if (organization.Suzerain != null)
            {
                line = new LineOfBudget
                {
                    Type = enLineOfBudgetType.SuzerainTax,
                    CoffersWillBe = -Constants.VassalTax,
                    Descripton = $"Передача налога сюзерену в {organization.Suzerain.Name}"
                };
                Lines.Add(line);
            }

            var commands = organizationCommands.Where(c => c.Type == Web.Enums.enCommandType.War);
            foreach (var item in commands)
            {
                line = new LineOfBudget
                {
                    Type = enLineOfBudgetType.War,
                    Warriors = -item.Warriors,
                    Descripton = $"Нападение на {item.Target.Name}",
                    Editable = true,
                    Deleteable = true,
                    CommandId = item.Id
                };
                Lines.Add(line);
            }

            var total = new LineOfBudget
            {
                Type = enLineOfBudgetType.Total,
                Coffers = Lines.Sum(l => l.Coffers),
                Warriors = Lines.Sum(l => l.Warriors),
                CoffersWillBe = Lines.Sum(l => l.CoffersWillBe),
                InvestmentsWillBe = Lines.Sum(l => l.InvestmentsWillBe),
                WarriorsWillBe = Lines.Sum(l => l.WarriorsWillBe),
                Descripton = $"ИТОГО: "
            };
            Lines.Add(total);
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
        Tax = 4,
        VassalTax = 5,
        SuzerainTax = 6,
        War = 7,
        Investments = 8,


        Total = 100
    }
}
