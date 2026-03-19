using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Processors;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Application.Colonies
{
    public interface IGetMyColonyProcessor : IProcessor<GetMyColonyCommand, GetMyColonyResult>;

    public class GetMyColonyProcessor(
        IColonyRepository colonyRepository)
        : IGetMyColonyProcessor
    {
        public async Task<GetMyColonyResult> Execute(GetMyColonyCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);

            return new GetMyColonyResult(colony);
        }
    }

    public record GetMyColonyCommand(long UserId) : IProcessorCommand;
    public record GetMyColonyResult(Colony? Colony) : IProcessorResult;
}
