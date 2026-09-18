using YAGO.World.Domain.Common;
using YAGO.World.Domain.Wiki;
using YAGO.World.Infrastructure.Datasets.Common;

namespace YAGO.World.Infrastructure.Datasets.Wiki
{
    public static class CharactersCassius
    {
        public static WikiArticle Get()
        {
            return new(WikiArticleConstants.GameplayCassius, WikiSection.Characters, 2, new DisplayInfo(
                "Кассиус Морн аль-Хаким",
                ImageSet.Cassius,
                [
                    "Кассиус Морн аль-Хаким родился в Дубае. Диплом Лондонской школы экономики, двенадцать лет в корпоративном праве и налоговом консалтинге.",
                    "Его называют самым холодным умом Пояса: он знает устав Консорциума лучше, чем сам Консорциум. За годы практики защищал интересы станций в десятках споров с корпорациями.",
                    "Управляет бюджетом колонии, налогами, контрактами и отчётностью. Отвечает за юридическую защиту колонии.",
                    "К деньгам относится как к инструменту: они должны работать, а не лежать в резервах. Требует точности в каждой цифре."
                ]));
        }
    }
}