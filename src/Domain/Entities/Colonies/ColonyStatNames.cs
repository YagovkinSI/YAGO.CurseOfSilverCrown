namespace YAGO.World.Domain.Entities.Colonies
{
    public static class ColonyStatNames
    {
        //Economic
        public const string Economic_Reserves = "Economic_Reserves";
        public const string Economic_Budget_Balance = "Economic_Budget_Balance";

        //Mood
        public const string Mood_Total = "Mood_Total";
        public const string Mood_Total_Balance = "Mood_Total_Balance";

        //AreaCapacity
        public const string AreaCapacity_Occupied = "AreaCapacity_Occupied";
        public const string AreaCapacity_Total = "AreaCapacity_Total";
        public const string AreaCapacity_Available = "AreaCapacity_Available";

        //Attractiveness
        public const string Attractiveness_Total = "Attractiveness_Total";

        //Laws
        public const string Laws_TaxLevel = "Laws_TaxLevel";
        public const string Laws_SocialGuaranteesLevel = "Laws_SocialGuaranteesLevel";

        //Industry
        //Industry_Minning
        public const string Industry_Administrative_Companies = $"{ColonyStatGroupNames.Industry}_Administrative_Companies";
        public const string Industry_Minning_Available = $"{ColonyStatGroupNames.Industry}_Minning_Available";
        public const string Industry_Minning_Companies = $"{ColonyStatGroupNames.Industry}_Minning_Companies";
        public const string Industry_Production_Companies = $"{ColonyStatGroupNames.Industry}_Production_Companies";
        public const string Industry_Service_Companies = $"{ColonyStatGroupNames.Industry}_Service_Companies";
        public const string Industry_Service_Need = $"{ColonyStatGroupNames.Industry}_Service_Need";

        //Population
        public const string Population_Total = "Population_Total";

        //Events
        public const string FirstWedding = "FirstWedding";

        //Time
        public const string CurrentWeek = "CurrentWeek";
        public const string EpisodeCount = "EpisodeCount";
    }
}
