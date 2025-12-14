using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies;
using YAGO.World.Domain.Cycles;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Cycles
{
    public class CycleService : ICycleService
    {
        private const int TimeoutBetweenCyclesInMinutes = 2;

        private readonly IColonyService _colonyService;
        private readonly ICycleRepository _cycleRepository;

        public CycleService(
            IColonyService colonyService,
            ICycleRepository cycleRepository)
        {
            _colonyService = colonyService;
            _cycleRepository = cycleRepository;
        }

        public async Task<Cycle> GetMyLastCycle(long userId, CancellationToken cancellationToken)
        {
            var myColony = await _colonyService.GetMyColony(userId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var cycle = await _cycleRepository.GetLast(myColony.Id, cancellationToken);

            if (cycle == null
                    || cycle.CompletedUtc < DateTime.UtcNow - TimeSpan.FromMinutes(TimeoutBetweenCyclesInMinutes))
                cycle = await _cycleRepository.CreateNew(myColony.Id, cancellationToken);

            return cycle;
        }
    }
}
