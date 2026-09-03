using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Wiki;

namespace YAGO.World.Host.Controllers.Wiki
{
    public static class WikiResponseMapping
    {
        public static WikiArticleResponse ToResponse(this WikiArticle article) =>
            new(
                article.Code,
                article.DisplayInfo.Name,
                article.DisplayInfo.ImageName,
                article.DisplayInfo.Description);

        public static IReadOnlyList<WikiSummaryResponse> ToSummaryResponse(
            this IEnumerable<WikiArticle> articles) =>
            articles.Select(article => new WikiSummaryResponse(
                article.Code,
                article.DisplayInfo.Name,
                IsRead: true)).ToList();
    }
}
