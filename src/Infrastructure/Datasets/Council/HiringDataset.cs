using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameActions;
using YAGO.World.Infrastructure.Datasets.Common;

namespace YAGO.World.Infrastructure.Datasets.Council
{
    internal static class HiringDataset
    {
        public static GameAction Get(string personCode)
        {
            return personCode switch
            {
                PersonConstants.Camilla => GetHireCamilla(),
                _ => throw new YagoNotFoundException(nameof(GameAction), personCode.ToString()),
            };
        }

        private static GameAction GetHireCamilla()
        {
            const int initialLoyalty = 40;
            var displayInfo = new DisplayInfo(
                "Камилла Селезнёва",
                ImageSet.Camilla,
                ["Камилла приняла пост администратора и берёт на себя координацию работы станции, внешние связи и поиск кадров."]);
            return new GameAction(
                effects:
                [
                    new GameEffect(GameEffectType.SetAdministrator, delta: initialLoyalty, code: PersonConstants.Camilla),
                    new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.GameplayCamilla),
                ],
                newEventCodes: [],
                displayInfoResult: displayInfo);
        }
    }
}