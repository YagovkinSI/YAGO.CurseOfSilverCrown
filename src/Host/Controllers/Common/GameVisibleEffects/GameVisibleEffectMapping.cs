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
        private static readonly GameEffectType[] _notVisible =
        [
            GameEffectType.SetColonyName,
            GameEffectType.SpendSolars,
            GameEffectType.SpendActionPoints,
            GameEffectType.ReformTaxLevel,
            GameEffectType.ReformSocialGuaranteesLevel,
            GameEffectType.SetFlagsFirstWedding
        ];

        public static IReadOnlyList<GameVisibleEffectResponse> ToVisibleEffectsResponse(
            this IEnumerable<GameEffect> source)
        {
            return source
                .Where(x => !_notVisible.Contains(x.Type))
                .Select(x => x.ToResponse())
                .ToList();
        }

        private static GameVisibleEffectResponse ToResponse(
            this GameEffect source)
        {
            var icon = source.Type.ToIcon();
            var label = source.Type switch
            {
                GameEffectType.AddSolars => "Получено соларов:",
                GameEffectType.SpendSolars => "Потрачено соларов:",
                GameEffectType.AddPublicDebt => "Долг увеличен на:",
                GameEffectType.AddActionPoints => "Получено ОД:",
                GameEffectType.SpendActionPoints => "Потрачено ОД:",
                GameEffectType.AddMood => "Получено доверия:",
                GameEffectType.AddBuildingsAdministrativeState => "Новых офисов:",
                GameEffectType.AddBuildingsMiningState => "Новых офисов:",
                _ => throw new YagoException($"Отображение эффекта не реализовано. Эффект: {source.Type}"),
            };
            var value = source.Delta.ToBeautifulString();
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