using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Handlers;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common.Exceptions;
using static YAGO.World.Application.Events.Commands.SetReadCommandHandler;

namespace YAGO.World.Application.Events.Commands
{
    public class SetReadCommandHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<SetReadCommand, HandlerResultEmpty>
    {
        public async Task<HandlerResultEmpty> Handle(SetReadCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException($"Отсутствует колония у пользователя с UserId={command.UserId}");

            var colonyEvents = colony.Events;
            var colonyEvent = colonyEvents.Single(x => x.EventId == command.EventId);
            colonyEvent.SetRead();

            await colonyRepository.Update(colony, cancellationToken);

            return new HandlerResultEmpty();
        }

        public record SetReadCommand(long UserId, string EventId) : IRequest<HandlerResultEmpty>;
    }
}
