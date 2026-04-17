using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Application.Services;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Application.Colonies.Commands.GetMyColony
{
    public class GetMyColonyCommandHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetMyColonyCommand, GetMyColonyResult>
    {
        public async Task<GetMyColonyResult> Handle(GetMyColonyCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);
            return new GetMyColonyResult(colony);
        }
    }

    public record GetMyColonyCommand(long UserId) : IRequest<GetMyColonyResult>;
    public record GetMyColonyResult(Colony? Colony);
}
