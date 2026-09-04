using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Wiki;

namespace YAGO.World.Application.Wiki.Queries.GetWikiArticle
{
    public class GetWikiArticleQueryHandler
        (IColonyRepository colonyRepository,
        IWikiRepository wikiRepository)
        : IRequestHandler<GetWikiArticleQuery, GetWikiArticleResult>
    {
        public async Task<GetWikiArticleResult> Handle(
            GetWikiArticleQuery query,
            CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(query.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");
            if (!colony.State.UnlockedWikiArticles.IsUnlocked(query.Code))
                throw new YagoException("Статья не найдена или недоступна.");

            var article = await wikiRepository.Get(query.Code, cancellationToken);
            if (!colony.State.UnlockedWikiArticles.IsRead(article.Code))
            {
                colony.State.UnlockedWikiArticles.MarkRead(article.Code);
                await colonyRepository.Update(colony, cancellationToken);
            }

            return new GetWikiArticleResult(article);
        }
    }

    public record GetWikiArticleQuery(long UserId, string Code) : IRequest<GetWikiArticleResult>;
    public record GetWikiArticleResult(WikiArticle Article);
}
