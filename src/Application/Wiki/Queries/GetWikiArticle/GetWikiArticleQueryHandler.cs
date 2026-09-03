using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Wiki;

namespace YAGO.World.Application.Wiki.Queries.GetWikiArticle
{
    public class GetWikiArticleQueryHandler(
        IWikiRepository wikiRepository)
        : IRequestHandler<GetWikiArticleQuery, GetWikiArticleResult>
    {
        public async Task<GetWikiArticleResult> Handle(
            GetWikiArticleQuery query,
            CancellationToken cancellationToken)
        {
            var article = await wikiRepository.Get(query.Code, cancellationToken);
            return new GetWikiArticleResult(article);
        }
    }

    public record GetWikiArticleQuery(string Code) : IRequest<GetWikiArticleResult>;

    public record GetWikiArticleResult(WikiArticle Article);
}
