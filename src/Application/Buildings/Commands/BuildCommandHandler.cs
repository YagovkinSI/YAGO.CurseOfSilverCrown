using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Entities.Colonies.Industries;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Mappings;

namespace YAGO.World.Application.Buildings.Commands
{
    public class BuildCommandHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<BuildCommand, BuildResult>
    {
        public async Task<BuildResult> Handle(BuildCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException($"Отсутствует колония у пользователя с UserId={command.UserId}");

            var industry = colony.State.Industries[command.Type];
            var buidingContext = colony.State.GetBuildingContext();
            var buiding = industry.GetBuilding(command.IsPrivate, buidingContext);
            var eventResult = new EventResult(
                buiding.Name,
                buiding.ImageName,
                buiding.Description,
                [], [], [], showForce: true);

            eventResult.SetMainParametersBefore(colony);

            buiding.Build(colony.State);

            eventResult.SetMainParametersAfter(colony);

            await colonyRepository.Update(colony, cancellationToken);

            return new BuildResult(eventResult);
        }
    }

    public record BuildCommand(long UserId, ColonyIndustryType Type, bool IsPrivate) : IRequest<BuildResult>;
    public record BuildResult(EventResult? EventResult);
}
