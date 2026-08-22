namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal record ColonyStatsEntity(
        ColonySolarsEntity Solars,
        ColonyActionPointsEntity ActionPoints,
        ColonyModulesEntity Modules,
        ColonyMoodEntity Mood,
        ColonyReformsEntity Reforms,
        ColonyIndustryEntity Industries,
        ColonyAchievementsEntity Achievements,
        ColonyCountersEntity Counters);

    internal record ColonySolarsEntity(
        double Reserve,
        double Income);

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

    internal record ColonyAchievementsEntity(
        bool RulerContractSigned,
        bool FirstWedding);

    internal record ColonyCountersEntity(
        double Turns);

}
