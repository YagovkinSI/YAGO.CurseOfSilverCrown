using System.Linq;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Infrastructure.APIModels.AspActions;
using YAGO.World.Infrastructure.APIModels.BudgetModels;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Units;
using YAGO.World.Infrastructure.Parameters;

namespace YAGO.World.Infrastructure.Promt
{
    public class PromtCreator
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepositoryCommads _repositoryCommads;

        public PromtCreator(
            ApplicationDbContext context,
            IRepositoryCommads repositoryCommads)
        {
            _context = context;
            _repositoryCommads = repositoryCommads;
        }

        public async Task<(string text, AspAction link)> Create(int organizationId)
        {
            var domain = await _context.Domains.FindAsync(organizationId);

            await _repositoryCommads.CheckAndFix(domain.Id);

            var budget = new Budget(_context, domain);
            var totalExpected = budget.Lines.Single(l => l.Type == BudgetLineType.Total).Coffers.ExpectedValue.Value;
            var isDeficit = totalExpected - domain.Gold < 0;

            return !isDeficit && domain.Gold - domain.Commands.Sum(c => c.Gold) > CoffersParameters.StartCount / 5
                ? ((string text, AspAction link))($"В казне владения имеется {domain.Gold} золотых монет. Вложите их грамотно.",
                    new AspAction("Commands", "Index", "Экономические и политические приказы"))
                : domain.Relations.Count == 0
                    ? ((string text, AspAction link))($"Выставите в отношениях какие владения вы готовы защищать. " +
                        $"Вы всегда защищаете своё владение и владения прямых вассалов.",
                        new AspAction("DomainRelations", "Index", "Управление отношениями"))
                    : domain.Units.All(u => u.Type == UnitCommandType.WarSupportDefense)
                        ? ((string text, AspAction link))($"Все отряды имеют приказы защиты. Возможно часть войск стоит отправить в атаку?",
                                new AspAction("Units", "Index", "Управление военными отрядами"))
                        : ((string text, AspAction link))($"Кажется все приказы отданы. Или нет?", null);
        }
    }
}
