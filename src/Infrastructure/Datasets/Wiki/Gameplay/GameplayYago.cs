using YAGO.World.Domain.Common;
using YAGO.World.Domain.Wiki;
using YAGO.World.Infrastructure.Datasets.Common;

namespace YAGO.World.Infrastructure.Datasets.Wiki
{
    public static class GameplayYago
    {
        public static WikiArticle Get()
        {
            return new(WikiArticleConstants.GameplayYago, WikiSection.Gameplay, 2, new DisplayInfo(
                "Проект YAGO",
                ImageSet.Yago,
                [
                    "В 2070 году Консорциум запустил программу «YAGO» (Yield and Governance Oversight - Надзор за Доходностью и Управлением), заменившую политику «Ответственного акционера». " +
                    "Это система рейтингования колоний, которая определяет полномочия правителя, его доступ к ресурсам и влияние внутри Консорциума.",
                    "Рейтинг YAGO — это не просто бюрократическая формальность. Он влияет на оплату и бонусы правителя, возможность получать дополнительные ссуды и даже право голоса в Совете развития Пояса. " +
                    "Понижение - инициирует инспекции и аудит и в конечном счёте может привести к отстранению правителя.",
                    "Система построена так, чтобы поощрять не только прибыль, но и репутацию. Консорциум заинтересован в стабильных, надёжных колониях, а не в краткосрочных успехах любой ценой."
                ]));
        }
    }
}