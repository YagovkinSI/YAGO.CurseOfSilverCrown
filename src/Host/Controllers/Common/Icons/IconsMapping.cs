using YAGO.World.Domain.GameActions;

namespace YAGO.World.Host.Controllers.Common.Icons
{
    public static class IconsMapping
    {
        public static string ToIcon(this GameRequirementType type)
        {
            return type switch
            {
                GameRequirementType.SolarsCanSpend => Icons.Solars,
                GameRequirementType.SolarsLessThan => Icons.Solars,
                GameRequirementType.ActionPointsCanSpend => Icons.ActionPoints,
                GameRequirementType.MoodLessThan => Icons.Mood,
                GameRequirementType.ModulesFreeCanSpend => Icons.Modules,
                GameRequirementType.ModulesUsedMoreThan => Icons.Modules,
                _ => Icons.Default,
            };
        }

        public static string ToIcon(this GameEffectType type)
        {
            return type switch
            {
                GameEffectType.SetColonyName => Icons.Default,
                GameEffectType.AddSolars => Icons.Solars,
                GameEffectType.SpendSolars => Icons.Solars,
                GameEffectType.AddPublicDebt => Icons.Solars,
                GameEffectType.AddActionPoints => Icons.ActionPoints,
                GameEffectType.SpendActionPoints => Icons.ActionPoints,
                GameEffectType.AddMood => Icons.Mood,
                GameEffectType.ReformTaxLevel => Icons.Default,
                GameEffectType.ReformSocialGuaranteesLevel => Icons.Default,
                GameEffectType.AddBuildingsAdministrativeState => Icons.Default,
                GameEffectType.AddBuildingsMiningState => Icons.Default,
                GameEffectType.SetFlagsFirstWedding => Icons.Default,
                _ => Icons.Default,
            };
        }
    }
}
