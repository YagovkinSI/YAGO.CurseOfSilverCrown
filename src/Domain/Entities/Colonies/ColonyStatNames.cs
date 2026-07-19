using System.Collections.Generic;

namespace YAGO.World.Domain.Entities.Colonies
{
    public static class ColonyStatNames
    {
        //ActionPoints
        public const string ActionPoints_Resourses = "ActionPoints_Reserves";
        public const string ActionPoints_Trend = "ActionPoints_Trend";

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
        //Industry_Administrative
        public const string Industry_Administrative_Companies_StateOwned = 
            $"{ColonyStatGroupNames.Industry}_Administrative_Companies_StateOwned";
        public const string Industry_Administrative_Companies_Private =
            $"{ColonyStatGroupNames.Industry}_Administrative_Companies_Private";

        //Industry_Minning
        public const string Industry_Minning_Available = $"{ColonyStatGroupNames.Industry}_Minning_Available";
        public const string Industry_Minning_Companies_StateOwned = 
            $"{ColonyStatGroupNames.Industry}_Minning_Companies_StateOwned";
        public const string Industry_Minning_Companies_Private =
            $"{ColonyStatGroupNames.Industry}_Minning_Companies_Private";

        //Industry_Production
        public const string Industry_Production_Companies_StateOwned = 
            $"{ColonyStatGroupNames.Industry}_Production_Companies_StateOwned";
        public const string Industry_Production_Companies_Private =
            $"{ColonyStatGroupNames.Industry}_Production_Companies_Private";

        //Industry_Service
        public const string Industry_Service_Companies_StateOwned = 
            $"{ColonyStatGroupNames.Industry}_Service_Companies_StateOwned";
        public const string Industry_Service_Companies_Private =
            $"{ColonyStatGroupNames.Industry}_Service_Companies_Private";
        public const string Industry_Service_Need = $"{ColonyStatGroupNames.Industry}_Service_Need";

        //Population
        public const string Population_Total = "Population_Total";

        //Events
        public const string FirstWedding = "FirstWedding";

        //Time
        public const string CurrentWeek = "CurrentWeek";

        public static IReadOnlyList<string> MainParameters =>
        [
            Economic_Reserves,
            Economic_Budget_Balance,
            Mood_Total,
            AreaCapacity_Occupied,
            Population_Total
        ];
    }
}
