using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Handlers;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Colonies.Buildings;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Buildings.Commands
{
    public class BuildCommandHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<BuildCommand, HandlerResultEmpty>
    {
        public async Task<HandlerResultEmpty> Handle(BuildCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException($"Отсутствует колония у пользователя с UserId={command.UserId}");

            colony.State.Buildings[command.Type].Build(command.IsPrivate, colony.State);

            return new HandlerResultEmpty();
        }
    }

    public record BuildCommand(long UserId, ColonyBuildingType Type, bool IsPrivate) : IRequest<HandlerResultEmpty>;
}
