using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Wiki;

namespace YAGO.World.Application.Wiki.Queries.GetWikiSummaries
{
    public class GetWikiSummariesQueryHandler
        (IColonyRepository colonyRepository,
        IWikiRepository wikiRepository)
        : IRequestHandler<GetWikiSummariesQuery, GetWikiSummariesResult>
    {
        public async Task<GetWikiSummariesResult> Handle(
            GetWikiSummariesQuery query,
            CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(query.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");

            var unlockedWikiArticles = colony.State.UnlockedWikiArticles;
            var articleCodes = unlockedWikiArticles.Values;
            var articles = await wikiRepository.GetAll(cancellationToken);
            var summaries = articleCodes
                .Select(x => new WikiSummaryDto(
                    articles.Single(y => y.Code == x.Key),
                    x.Value))
                .ToList();
            return new GetWikiSummariesResult(summaries);
        }
    }

    public record GetWikiSummariesQuery(long UserId) : IRequest<GetWikiSummariesResult>;
    public record GetWikiSummariesResult(IReadOnlyList<WikiSummaryDto> Summaries);
    public record WikiSummaryDto(WikiArticle Article, bool IsRead);
}
