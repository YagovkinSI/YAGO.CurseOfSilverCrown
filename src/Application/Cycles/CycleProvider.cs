using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies;
using YAGO.World.Domain.Cycles;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Cycles
{
    public class CycleProvider : ICycleProvider
    {
        private const int TimeoutBetweenCyclesInSeconds = 12;

        private readonly IColonyService _colonyService;
        private readonly ICycleRepository _cycleRepository;

        public CycleProvider(
            IColonyService colonyService,
            ICycleRepository cycleRepository)
        {
            _colonyService = colonyService;
            _cycleRepository = cycleRepository;
        }

        public async Task<Cycle?> Get(GetCycleCommand command, CancellationToken cancellationToken)
        {
            var myColony = await _colonyService.GetMyColony(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var cycle = await _cycleRepository.GetLast(myColony.Id, cancellationToken);

            if (cycle == null
                    || (cycle.State == CycleState.Completed
                        && cycle.RunAtUtc < DateTime.UtcNow - TimeSpan.FromSeconds(TimeoutBetweenCyclesInSeconds)))
            {
                cycle = await _cycleRepository.CreateNew(myColony.Id, cancellationToken);
            }

            return cycle;
        }
    }
}
