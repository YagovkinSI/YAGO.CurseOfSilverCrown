using System;
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
        private const int TimeoutBetweenCyclesInMinutes = 2;

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
            if (cycle == null || cycle.CompletedUtc < DateTime.UtcNow - TimeSpan.FromMinutes(TimeoutBetweenCyclesInMinutes))
                cycle = await _cycleRepository.CreateNew(myColony.Id, cancellationToken);

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
