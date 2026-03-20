using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Processors;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies.Commands.CreateColony
{
    public class CreateColonyCommandHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<CreateColonyCommand, ProcessorResultEmpty>
    {
        public async Task<ProcessorResultEmpty> Handle(CreateColonyCommand command, CancellationToken cancellationToken)
        {
            var userColony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (userColony != null)
                throw new YagoException(string.Format("Пользователь уже имеет колонию '{0}'.", userColony.Name));

            var isNameAvailable = await colonyRepository.IsNameAvailable(command.ColonyName, cancellationToken);
            if (!isNameAvailable)
                throw new YagoException(string.Format("Название колонии '{0}' уже занято.", command.ColonyName));

            var colony = Colony.CreateNew(command.UserId, command.ColonyName, command.GavernorType);
            await colonyRepository.Add(colony, cancellationToken);

            return new ProcessorResultEmpty();
        }
    }
    public record CreateColonyCommand(long UserId, string ColonyName, CodeOfLaws GavernorType) : IRequest<ProcessorResultEmpty>;
}
