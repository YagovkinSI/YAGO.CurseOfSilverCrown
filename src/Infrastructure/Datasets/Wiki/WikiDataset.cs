using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Wiki;

namespace YAGO.World.Infrastructure.Datasets.Wiki
{
    internal static class WikiDataset
    {
        public static IReadOnlyList<WikiArticle> All =>
        [
            LifeSolar.Get(),
            LifeShareholders.Get(),
            LifeEarth2070.Get(),
            LifeSpaceElevator.Get(),

            HistoryQuatlas.Get(),
            HistoryHelium3.Get(),
            HistoryFirebird.Get(),
            HistoryNanotubes.Get(),
            HistorySpaceTreaty.Get(),
            HistoryKesslerCascade.Get(),
            HistoryGoldenRing.Get(),
            HistoryColonization.Get(),

            StationDawn.Get(),
            StationResolute.Get(),

            FactionAvalon.Get(),
            FactionPhoenix.Get(),
            FactionOpz.Get(),

            CharactersCamilla.Get(),
            CharactersCassius.Get(),
            CharactersLien.Get(),
            CharactersDarius.Get(),

            GameplayYago.Get(),
        ];

        public static WikiArticle Get(string code)
        {
            var article = All.SingleOrDefault(x => x.Code == code)
                ?? throw new YagoNotFoundException(nameof(WikiArticle), code.ToString());
            return article;
        }
    }
}