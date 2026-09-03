using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Wiki;

namespace YAGO.World.Infrastructure.Datasets.Wiki
{
    internal class WikiRepository : IWikiRepository
    {
        public Task<IReadOnlyList<WikiArticle>> GetAll(CancellationToken cancellationToken)
        {
            var result = WikiDataset.All;
            return Task.FromResult(result);
        }

        public Task<WikiArticle> Get(string code, CancellationToken cancellationToken)
        {
            var result = WikiDataset.Get(code);
            return Task.FromResult(result);
        }
    }
}
