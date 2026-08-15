using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Application.Events.Commands
{
    public class SetReadCommandHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<SetReadCommand, Unit>
    {
        public async Task<Unit> Handle(SetReadCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException($"Отсутствует колония у пользователя с UserId={command.UserId}");

            var colonyEvents = colony.Events;
            var colonyEvent = colonyEvents[command.EventId];
            colonyEvent.SetRead();

            await colonyRepository.Update(colony, cancellationToken);

            return new Unit();
        }
    }

    public record SetReadCommand(long UserId, string EventId) : IRequest<Unit>;
}
