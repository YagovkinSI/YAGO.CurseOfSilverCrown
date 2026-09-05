using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Persons;

namespace YAGO.World.Application.Interfaces.Repository
{
    public interface IPersonRepository
    {
        Task<Person> Get(string code, CancellationToken cancellationToken);
    }
}