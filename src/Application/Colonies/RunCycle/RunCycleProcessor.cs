using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cycles;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Companies;
using YAGO.World.Domain.Episodes;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Ships;

namespace YAGO.World.Application.Colonies.RunCycle
{
    public class RunCycleProcessor : IRunCycleProcessor
    {
        private readonly IColonyService _colonyService;
        private readonly ICycleService _cycleService;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public RunCycleProcessor(
            IColonyService colonyService,
            ICycleService cycleService,
            IUnitOfWorkRepository unitOfWorkRepository)
        {
            _colonyService = colonyService;
            _cycleService = cycleService;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<RunCycleResult> Execute(RunCycleCommand command, CancellationToken cancellationToken)
        {
            var userId = command.UserId;

            var colony = await _colonyService.GetMyColony(userId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var currentEpisodeId = colony.EpisodeId;
            if (currentEpisodeId != null)
                return GetEpisode(currentEpisodeId.Value, colony);

            var lastCycle = await _cycleService.GetMyLastCycle(userId, cancellationToken)
                ?? throw new YagoException("Цикл отсутствует. Вероятно нет созданной колонии.");

            if (lastCycle.State == Domain.Cycles.CycleState.Completed)
                throw new YagoException("Цикл завершен. Дождитесь следующего цикла не более двух минут.");

            var ship = ShipDataset.GetShip(colony.ShipId);
            var companies = CompanyDataset.GetCompanies(colony.CompanyIds);
            var notification = lastCycle.RunCycle(colony, companies, ship);

            var list = new List<IEntity>
            {
                colony,
                lastCycle
            };
            await _unitOfWorkRepository.UpdateInTransactionAsync(list, cancellationToken);

            var myCycle = await _cycleService.GetMyLastCycle(userId, cancellationToken);

            var colonyWithDeatails = new ColonyWithDetails(colony, ship, companies);
            var episode = new Episode(id: null, [notification], сhoiceLabel: null, сhoice: null);
            return new RunCycleResult(episode, colonyWithDeatails, myCycle);
        }

        private RunCycleResult GetEpisode(long episodeId, Domain.Colonies.Colony colony)
        {
            var ship = ShipDataset.GetShip(colony.ShipId);
            var companies = CompanyDataset.GetCompanies(colony.CompanyIds);
            var colonyWithDeatails = new ColonyWithDetails(colony, ship, companies);

            var episode = EpisodeDataset.Get(episodeId);
            return new RunCycleResult(episode, colonyWithDeatails, myCycle: null);
        }
    }
}
