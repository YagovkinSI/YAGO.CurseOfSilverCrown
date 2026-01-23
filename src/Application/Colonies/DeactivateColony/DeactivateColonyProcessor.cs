using System.Threading;
using System.Threading.Tasks;

namespace YAGO.World.Application.Colonies.DeactivateColony
{
    public class DeactivateColonyProcessor : IDeactivateColonyProcessor
    {
        private readonly IColonyRepository _colonyRepository;

        public DeactivateColonyProcessor(
            IColonyRepository colonyRepository)
        {
            _colonyRepository = colonyRepository;
        }

        public async Task<DeactivateColonyResult> Execute(DeactivateColonyCommand command, CancellationToken cancellationToken)
        {
            var userColony = await _colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (userColony == null)
                return new DeactivateColonyResult();

            userColony.Deactivate();

            await _colonyRepository.Update(userColony, cancellationToken);

            return new DeactivateColonyResult();
        }
    }
}
