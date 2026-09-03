using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Wiki;

namespace YAGO.World.Application.Wiki.Queries.GetWikiSummaries
{
    public class GetWikiSummariesQueryHandler(
        IWikiRepository wikiRepository)
        : IRequestHandler<GetWikiSummariesQuery, GetWikiSummariesResult>
    {
        public async Task<GetWikiSummariesResult> Handle(
            GetWikiSummariesQuery query,
            CancellationToken cancellationToken)
        {
            var articles = await wikiRepository.GetAll(cancellationToken);
            return new GetWikiSummariesResult(articles);
        }
    }

    public record GetWikiSummariesQuery() : IRequest<GetWikiSummariesResult>;

    public record GetWikiSummariesResult(IReadOnlyList<WikiArticle> Articles);
}
