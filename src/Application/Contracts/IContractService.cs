using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Contracts;

namespace YAGO.World.Application.Contracts
{
    public interface IContractService
    {
        Task<Contract?> GetContract(long contractId, CancellationToken cancellationToken);
    }
}
