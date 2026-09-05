using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Persons;

namespace YAGO.World.Infrastructure.Datasets.Persons
{
    internal class PersonRepository : IPersonRepository
    {
        public Task<Person> Get(string code, CancellationToken cancellationToken)
        {
            var result = PersonDataset.Get(code);
            return Task.FromResult(result);
        }
    }
}