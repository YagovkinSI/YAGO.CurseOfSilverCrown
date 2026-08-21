using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameActions;

namespace YAGO.World.Application.Colonies.Commands
{
    public class SetReformCommandHandler(
        IColonyRepository colonyRepository,
        IReformRepository reformRepository)
        : IRequestHandler<SetReformCommand, SetReformResult>
    {
        public async Task<SetReformResult> Handle(SetReformCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var reform = await reformRepository.Get(command.ReformCode, cancellationToken);
            var displayInfo = new DisplayInfo(
                reform.DisplayInfo.Name,
                reform.DisplayInfo.ImageName,
                description: []);
            var eventResult = new GameActionResult(
                displayInfo,
                showForce: false);
            eventResult.SetMainParametersBefore(colony);

            reform.Action.Aplly(colony, command.ReformValue);

            eventResult.SetMainParametersAfter(colony);

            await colonyRepository.Update(colony, cancellationToken);

            return new SetReformResult(eventResult);
        }
    }

    public record SetReformCommand(long UserId, string ReformCode, string ReformValue) : IRequest<SetReformResult>;
    public record SetReformResult(GameActionResult ActionResult);
}
