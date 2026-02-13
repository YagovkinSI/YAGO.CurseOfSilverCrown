using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Companies;

namespace YAGO.World.Application.Contracts
{
    public interface IContractService
    {
        Task<Company?> GetContract(long contractId, CancellationToken cancellationToken);
    }
}
