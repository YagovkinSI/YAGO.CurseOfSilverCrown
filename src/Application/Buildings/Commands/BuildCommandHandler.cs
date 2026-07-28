using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Colonies.Buildings;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;

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

            var buiding = colony.State.Buildings[command.Type];
            var buidingSettings = buiding.GetSettings();
            var eventResult = new EventResult(
                buidingSettings.Name,
                buidingSettings.ImageName,
                buidingSettings.Description,
                [], [], [], showForce: true);

            eventResult.SetMainParametersBefore(colony);

            buiding.Build(command.IsPrivate, colony.State);

            eventResult.SetMainParametersAfter(colony);

            await colonyRepository.Update(colony, cancellationToken);

            return new BuildResult(eventResult);
        }
    }

    public record BuildCommand(long UserId, ColonyBuildingType Type, bool IsPrivate) : IRequest<BuildResult>;
    public record BuildResult(EventResult? EventResult);
}
