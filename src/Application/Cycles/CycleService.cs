using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies;
using YAGO.World.Application.Users;
using YAGO.World.Domain.Cycles;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Cycles
{
    public class CycleService : ICycleService
    {
        public readonly IUserService _userService;
        private readonly IColonyService _colonyService;
        private readonly ICycleRepository _cycleRepository;

        public CycleService(
            IUserService userService,
            IColonyService colonyService,
            ICycleRepository cycleRepository)
        {
            _userService = userService;
            _colonyService = colonyService;
            _cycleRepository = cycleRepository;
        }

        public async Task<Cycle?> GetMyLastCycle(long userId, CancellationToken cancellationToken)
        {
            var myColony = await _colonyService.GetMyColony(userId, cancellationToken);
            if (myColony == null)
                return null;

            var cycle = await _cycleRepository.GetLast(myColony.Id, cancellationToken);
            if (cycle != null && cycle.Status != CycleStatus.Completed)
                return cycle;

            if (cycle == null || cycle.IsReadyForNewCycle())
            {
                var newCycle = Cycle.CreateNew(myColony.Id);
                cycle = await _cycleRepository.CreateNew(newCycle, cancellationToken);
            }

            return cycle;
        }

        public async Task<Cycle?> RunCycle(long userId, CancellationToken cancellationToken)
        {
            var colonyWithShipAndBuildings = await _colonyService.GetMyColonyWithShipAndBuildings(userId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var lastCycle = await GetMyLastCycle(userId, cancellationToken)
                ?? throw new YagoException("Цикл отсутствует. Вероятно нет созданной колонии.");

            lastCycle.SetCompleted();
            colonyWithShipAndBuildings.AddIncome();

            return await _cycleRepository.Update(lastCycle, colonyWithShipAndBuildings.Colony, cancellationToken);
        }
    }
}
