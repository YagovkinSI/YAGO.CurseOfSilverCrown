using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameActions;
using YAGO.World.Host.Controllers.Common.Extensions;
using YAGO.World.Application.Common.Extensions;
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
            var label = GetLabel(source);
            if (label == null)
                return null;
            var value = source.Delta.ToBeautifulString(setPlus: true);
            var effectColor = GetEffectColor(source);

            return new GameVisibleEffectResponse(
                icon,
                label,
                value,
                effectColor,
                Url: null);
        }

        private static string? GetLabel(GameEffect source)
        {
            return source.Type switch
            {
                GameEffectType.SetColonyName => null,
                GameEffectType.AddSolars => "Солары",
                GameEffectType.SpendSolars => null,
                GameEffectType.AddPublicDebt => "Долг",
                GameEffectType.AddActionPoints => "Очки действий",
                GameEffectType.SpendActionPoints => null,
                GameEffectType.AddMood => "Доверие",
                GameEffectType.AddBuildingsAdministrativeState => null,
                GameEffectType.AddBuildingsMiningState => null,
                GameEffectType.ReformTaxLevel => null,
                GameEffectType.ReformSocialGuaranteesLevel => null,
                GameEffectType.SetAchievement => null,
                _ => throw new YagoException($"Отображение эффекта не реализовано. Эффект: {source.Type}"),
            };
        }

        private static string GetEffectColor(GameEffect source)
        {
            return source.Type switch
            {
                GameEffectType.AddSolars => GetEffectColorByBool(source.Delta > 0),
                GameEffectType.SpendSolars => GetEffectColorByBool(source.Delta < 0),
                GameEffectType.AddPublicDebt => GetEffectColorByBool(source.Delta < 0),
                GameEffectType.AddActionPoints => GetEffectColorByBool(source.Delta > 0),
                GameEffectType.SpendActionPoints => GetEffectColorByBool(source.Delta < 0),
                GameEffectType.AddMood => GetEffectColorByBool(source.Delta > 0),
                GameEffectType.AddBuildingsAdministrativeState => GetEffectColorByBool(source.Delta > 0),
                GameEffectType.AddBuildingsMiningState => GetEffectColorByBool(source.Delta > 0),
                GameEffectType.SetColonyName => EffectColorConstats.Neutral,
                GameEffectType.ReformTaxLevel => EffectColorConstats.Neutral,
                GameEffectType.ReformSocialGuaranteesLevel => EffectColorConstats.Neutral,
                GameEffectType.SetAchievement => EffectColorConstats.Neutral,
                _ => throw new YagoException($"Отображение эффекта не реализовано. Эффект: {source.Type}"),
            };
        }

        private static string GetEffectColorByBool(bool isPositive)
        {
            return isPositive ? EffectColorConstats.Positive : EffectColorConstats.Negative;
        }
    }
}