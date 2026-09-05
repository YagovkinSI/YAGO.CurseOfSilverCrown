using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Wiki;

namespace YAGO.World.Application.Interfaces.Repository
{
    public interface IWikiRepository
    {
        Task<IReadOnlyList<WikiArticle>> GetAll(CancellationToken cancellationToken);
        Task<WikiArticle> Get(string code, CancellationToken cancellationToken);
    }
}
