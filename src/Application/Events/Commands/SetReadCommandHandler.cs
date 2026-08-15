using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Application.Events.Commands
{
    public class SetReadCommandHandler(
        IColonyRepository colonyRepository,
        IColonyEventRepository colonyEventRepository)
        : IRequestHandler<SetReadCommand, Unit>
    {
        public async Task<Unit> Handle(SetReadCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");
            var colonyEvent = await colonyEventRepository.Find(command.ColonyEventId, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(ColonyEvent), command.ColonyEventId.ToString());
            if (colonyEvent.ColonyId != colony.Id)
                throw new YagoNotVerifyOwnershipException(nameof(ColonyEvent), command.ColonyEventId.ToString());

            colonyEvent.SetRead();

            await colonyEventRepository.Update(colonyEvent, cancellationToken);

            return new Unit();
        }
    }

    public record SetReadCommand(long UserId, long ColonyEventId) : IRequest<Unit>;
}
