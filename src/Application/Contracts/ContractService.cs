using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Companies;

namespace YAGO.World.Application.Contracts
{
    public class ContractService : IContractService
    {
        public Task<Company?> GetContract(long contractId, CancellationToken cancellationToken)
        {
            var result = CompanyDataset.Get().FirstOrDefault(x => x.Id == contractId);
            return Task.FromResult(result);
        }
    }
}
