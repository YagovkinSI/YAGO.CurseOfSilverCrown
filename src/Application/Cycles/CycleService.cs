using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cycles;
using YAGO.World.Application.Users;
using YAGO.World.Domain.Cycles;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies
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

        public async Task<Cycle?> GetMyLastCycle(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            _ = await _userService.GetMyUser(claimsPrincipal, cancellationToken)
                ?? throw new YagoNotAuthorizedException();

            var myColony = await _colonyService.GetMyColony(claimsPrincipal, cancellationToken);
            if (myColony == null)
                return null;

            var cycle = await _cycleRepository.GetLast(myColony.Id, cancellationToken);
            if (cycle == null || cycle.CompletedUtc < DateTime.UtcNow - TimeSpan.FromMinutes(TimeoutBetweenCyclesInMinutes))
            {
                cycle = await _cycleRepository.CreateNew(myColony.Id, cancellationToken);
            }

            return cycle;
        }

        public async Task<Cycle?> RunCycle(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var lastCycle = await GetMyLastCycle(claimsPrincipal, cancellationToken);
            if (lastCycle == null)
                throw new YagoException("Цикл отсутствует. Вероятно нет созданной колонии.");

            if (lastCycle.CompletedUtc != null)
                throw new YagoException("Цикл уже завершён, необходимо дождаться нового цикла.");

            await _cycleRepository.SetComplited(lastCycle.Id, cancellationToken);

            return await _cycleRepository.Find(lastCycle.Id, cancellationToken);


        }
    }
}
