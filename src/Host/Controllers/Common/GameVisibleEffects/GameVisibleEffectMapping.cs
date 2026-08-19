using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameActions;
using YAGO.World.Host.Controllers.Common.Extensions;
using YAGO.World.Host.Controllers.Common.Icons;

namespace YAGO.World.Host.Controllers.Common.GameVisibleEffects
{
    internal static class GameVisibleEffectMapping
    {
        public static IReadOnlyList<GameVisibleEffectResponse> ToVisibleEffectsResponse(
            this IEnumerable<GameEffect> source)
        {
            return source
                .Select(x => x.ToResponse())
                .Where(x => x != null)
                .Select(x => x!)
                .ToList();
        }

        private static GameVisibleEffectResponse? ToResponse(
            this GameEffect source)
        {
            var icon = source.Type.ToIcon();
            var label = source.Type switch
            {
                GameEffectType.SetColonyName => null,
                GameEffectType.AddSolars => "Солары:",
                GameEffectType.SpendSolars => null,
                GameEffectType.AddPublicDebt => "Долг:",
                GameEffectType.AddActionPoints => "Очки действий:",
                GameEffectType.SpendActionPoints => null,
                GameEffectType.AddMood => "Доверие:",
                GameEffectType.AddBuildingsAdministrativeState => null,
                GameEffectType.AddBuildingsMiningState => null,
                GameEffectType.ReformTaxLevel => null,
                GameEffectType.ReformSocialGuaranteesLevel => null,
                GameEffectType.SetFlagsFirstWedding => null,
                _ => throw new YagoException($"Отображение эффекта не реализовано. Эффект: {source.Type}"),
            };
            if (label == null)
                return null;
            var value = source.Delta.ToBeautifulString(setPlus: true); 
            var status = source.Type switch
            {
                GameEffectType.AddSolars => source.Delta > 0,
                GameEffectType.SpendSolars => source.Delta < 0,
                GameEffectType.AddPublicDebt => source.Delta < 0,
                GameEffectType.AddActionPoints => source.Delta > 0,
                GameEffectType.SpendActionPoints => source.Delta < 0,
                GameEffectType.AddMood => source.Delta > 0,
                GameEffectType.AddBuildingsAdministrativeState => source.Delta > 0,
                GameEffectType.AddBuildingsMiningState => source.Delta > 0,
                _ => throw new YagoException($"Отображение эффекта не реализовано. Эффект: {source.Type}"),
            };

            return new GameVisibleEffectResponse(
                icon,
                label,
                value,
                status,
                Url: null,
                InfoUrl: null);
        }
    }
}