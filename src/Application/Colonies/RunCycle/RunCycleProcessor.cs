using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cycles;
using YAGO.World.Domain.Colonies.Companies;
using YAGO.World.Domain.Colonies.Ships;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Cycles;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies.RunCycle
{
    public class RunCycleProcessor : IRunCycleProcessor
    {
        private readonly IColonyService _colonyService;
        private readonly ICycleProvider _cycleProvider;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public RunCycleProcessor(
            IColonyService colonyService,
            ICycleProvider cycleService,
            IUnitOfWorkRepository unitOfWorkRepository)
        {
            _colonyService = colonyService;
            _cycleProvider = cycleService;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<RunCycleResult> Execute(RunCycleCommand command, CancellationToken cancellationToken)
        {
            var userId = command.UserId;

            var colony = await _colonyService.GetMyColony(userId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var lastCycle = await GetLastCycle(userId, cancellationToken);

            if (lastCycle.State == Domain.Cycles.CycleState.Completed)
                throw new YagoException("Цикл завершен. Дождитесь следующего цикла не более двух минут.");

            var ship = ShipDataset.GetShip(colony.ShipId);
            var colonyStats = colony.Stats;
            var companies = CompanyDataset.GetCompanies(colonyStats.CompanyIds);
            var episode = lastCycle.RunCycle(colony, companies, ship);

            var list = new List<IEntity>
            {
                colony,
                lastCycle
            };
            await _unitOfWorkRepository.UpdateInTransactionAsync(list, cancellationToken);

            var myCycle = await GetLastCycle(userId, cancellationToken);

            var colonyWithDeatails = new ColonyWithDetails(colony, ship, companies);
            return new RunCycleResult(episode, colonyWithDeatails, myCycle);
        }

        private async Task<Cycle> GetLastCycle(long userId, CancellationToken cancellationToken)
        {
            var command = new GetCycleCommand(userId);
            return await _cycleProvider.Get(command, cancellationToken)
                ?? throw new YagoException("Цикл отсутствует. Вероятно нет созданной колонии.");
        }
    }
}
