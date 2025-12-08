using System.Threading;
using System.Threading.Tasks;

namespace YAGO.World.Application.Common.Processors
{
    public interface IProcessor<TCommand, TResult>
        where TCommand : IProcessorCommand
        where TResult : IProcessorResult
    {
        Task<TResult> Execute(TCommand command, CancellationToken cancellationToken);
    }
}
