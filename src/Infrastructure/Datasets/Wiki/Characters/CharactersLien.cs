using YAGO.World.Domain.Common;
using YAGO.World.Domain.Wiki;
using YAGO.World.Infrastructure.Datasets.Common;

namespace YAGO.World.Infrastructure.Datasets.Wiki
{
    public static class CharactersLien
    {
        public static WikiArticle Get()
        {
            return new(WikiArticleConstants.GameplayLien, WikiSection.Characters, 3, new DisplayInfo(
                "Лиен Чжан",
                ImageSet.Lien,
                [
                    "Лиен Чжан родилась в Циндао. В шестнадцать лет улетела на Луну по программе RAS. Прошла путь от разнорабочей до старшего инженера.",
                    "Ценит безопасность выше эффективности: на её станциях реактор никогда не работает на пределе.",
                    "Отвечает за реактор, системы жизнеобеспечения и техническое состояние станции. Следит за безопасностью и предотвращает аварии.",
                    "Рабочие её уважают: она знает каждую систему снаружи и изнутри и не экономит на профилактике."
                ]));
        }
    }
}