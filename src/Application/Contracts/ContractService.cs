using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Contracts;

namespace YAGO.World.Application.Contracts
{
    public class ContractService : IContractService
    {
        public Task<Contract?> GetContract(long contractId, CancellationToken cancellationToken)
        {
            var result = ContractDataset.Get().FirstOrDefault(x => x.Id == contractId);
            return Task.FromResult(result);
        }
    }
}
