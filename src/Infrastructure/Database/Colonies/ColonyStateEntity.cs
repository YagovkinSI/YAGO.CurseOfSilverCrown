using System.Collections.Generic;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal record ColonyStateEntity(
        double Solars,
        ColonyActionPointsEntity ActionPoints,
        ColonyModulesEntity Modules,
        ColonyMoodEntity Mood,
        ColonyReformsEntity Reforms,
        ColonyIndustryEntity Industries,
        IEnumerable<string> Achievements,
        IReadOnlyDictionary<string, bool> UnlockedWikiArticles,
        ColonyCountersEntity Counters,
        ColonyCouncilEntity Council);

    internal record ColonyActionPointsEntity(
        int Reserve,
        int Income);

    internal record ColonyModulesEntity(
        double Total,
        double Used);

    internal record ColonyMoodEntity(
        double Reserve);

    internal record ColonyReformsEntity(
        double TaxLevel,
        double SocialGuaranteesLevel,
        double PublicDebt);

    internal record ColonyIndustryEntity(
        ColonyBuildingsEntity Administrative,
        ColonyBuildingsEntity Mining,
        ColonyBuildingsEntity Production,
        ColonyBuildingsEntity Service);

    internal record ColonyBuildingsEntity(
        double State,
        double Private);

    internal record ColonyCountersEntity(
        double Turns);

    internal record ColonyCouncilEntity(
        ColonyCouncilAdvisorEntity? Administrator,
        ColonyCouncilAdvisorEntity? Engineer,
        ColonyCouncilAdvisorEntity? Financier,
        ColonyCouncilAdvisorEntity? Social);

    internal record ColonyCouncilAdvisorEntity(
        string Code,
        int Loyalty);

}
