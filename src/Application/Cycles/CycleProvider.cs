using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Cycles
{
    public class CycleProvider : ICycleProvider
    {
        private const int TimeoutBetweenCyclesInSeconds = 12;

        private readonly IColonyRepository _colonyRepository;
        private readonly ICycleRepository _cycleRepository;

        public CycleProvider(
            IColonyRepository colonyRepository,
            ICycleRepository cycleRepository)
        {
            _colonyRepository = colonyRepository;
            _cycleRepository = cycleRepository;
        }

        public async Task<Cycle?> Get(GetCycleCommand command, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var cycle = await _cycleRepository.GetLast(colony.Id, cancellationToken);

            if (cycle == null
                    || (cycle.State == CycleState.Completed
                        && cycle.RunAtUtc < DateTime.UtcNow - TimeSpan.FromSeconds(TimeoutBetweenCyclesInSeconds)))
            {
                cycle = await _cycleRepository.CreateNew(colony.Id, cancellationToken);
            }

            return cycle;
        }
    }
}
