using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.GameActions;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameActions;

namespace YAGO.World.Application.Colonies.Commands
{
    public class SetReformCommandHandler(
        IColonyRepository colonyRepository,
        IReformRepository reformRepository,
        IApplyGameActionService applyGameActionService)
        : IRequestHandler<SetReformCommand, SetReformResult>
    {
        public async Task<SetReformResult> Handle(SetReformCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var reform = await reformRepository.Get(command.ReformCode, cancellationToken);
            var eventResult = applyGameActionService.Apply(
                reform.Action, colony, command.ReformValue);

            await colonyRepository.Update(colony, cancellationToken);

            return new SetReformResult(eventResult.GameActionResult);
        }
    }

    public record SetReformCommand(long UserId, string ReformCode, string ReformValue) : IRequest<SetReformResult>;
    public record SetReformResult(GameActionResult ActionResult);
}
