namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal record ColonyStatsEntity(
        ColonySolarsEntity Solars,
        ColonyReformPointsEntity ReformPoints,
        ColonyModulesEntity Modules,
        ColonyMoodEntity Mood,
        ColonyReformsEntity Reforms,
        ColonyIndustryEntity Industries,
        ColonyFlagsEntity Flags,
        ColonyCountersEntity Counters);

    internal record ColonySolarsEntity(
        double Reserve,
        double Income);

    internal record ColonyReformPointsEntity(
        double Reserve,
        double Income);

    internal record ColonyModulesEntity(
        double Total,
        double Used);

    internal record ColonyMoodEntity(
        double Reserve);

    internal record ColonyReformsEntity(
        double TaxLevel,
        double SocialGuaranteesLevel);

    internal record ColonyIndustryEntity(
        ColonyBuildingsEntity Administrative,
        ColonyBuildingsEntity Mining,
        ColonyBuildingsEntity Production,
        ColonyBuildingsEntity Service);

    internal record ColonyBuildingsEntity(
        double State,
        double Private);

    internal record ColonyFlagsEntity(
        double FirstWedding);

    internal record ColonyCountersEntity(
        double Turns);

}
