using System.Threading;
using System.Threading.Tasks;

namespace YAGO.World.Application.Common.Processors
{
    public interface IProvider<TCommand, TResult>
        where TCommand : IProcessorCommand
    {
        Task<TResult> Execute(TCommand command, CancellationToken cancellationToken);
    }
}
