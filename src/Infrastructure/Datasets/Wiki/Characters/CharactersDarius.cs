using YAGO.World.Domain.Common;
using YAGO.World.Domain.Wiki;
using YAGO.World.Infrastructure.Datasets.Common;

namespace YAGO.World.Infrastructure.Datasets.Wiki
{
    public static class CharactersDarius
    {
        public static WikiArticle Get()
        {
            return new(WikiArticleConstants.GameplayDarius, WikiSection.Characters, 4, new DisplayInfo(
                "Дариус «Док» Уэбб",
                ImageSet.Darius,
                [
                    "Дариус «Док» Уэбб родился в Хьюстоне. Пятнадцать лет служил в миграционной службе ОПЗ на Луне.",
                    "Считает, что каждый заслуживает шанса, — и умеет выстроить процесс так, чтобы новичок не потерялся на станции.",
                    "Отвечает за найм, удержание колонистов и внутренний климат. Решает конфликты между работниками.",
                    "За плечами — тысячи переселений. Говорит, что лучшая статистика — та, где люди остаются добровольно."
                ]));
        }
    }
}