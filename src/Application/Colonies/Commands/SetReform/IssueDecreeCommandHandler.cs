using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Application.Colonies.Commands.SetReform
{
    public class SetReformCommandHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<SetReformCommand, SetReformResult>
    {
        public async Task<SetReformResult> Handle(SetReformCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var colonyState = colony.State;
            var reform = colonyState.GetReform(command.ReformId);

            var eventResult = new EventResult(
                reform.Name,
                reform.Image,
                text: [],
                [], [], [], showForce: true);
            eventResult.SetMainParametersBefore(colony);

            colonyState.SetReform(reform);

            eventResult.SetMainParametersAfter(colony);

            await colonyRepository.Update(colony, cancellationToken);

            return new SetReformResult(eventResult);
        }
    }

    public record SetReformCommand(long UserId, long ReformId) : IRequest<SetReformResult>;
    public record SetReformResult(EventResult EventResult);
}
