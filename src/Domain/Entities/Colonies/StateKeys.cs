using System.Collections.Generic;

namespace YAGO.World.Domain.Entities.Colonies
{
    public static class StateKeys
    {
        public static class Solars
        {
            public const string Reserve = "solars.reserve";
            public const string Income = "solars.income";
        }

        public static class ReformPoints
        {
            public const string Reserve = "reform_points.reserve";
            public const string Income = "reform_points.income";
        }

        public static class Modules
        {
            public const string Total = "modules.total";
            public const string Used = "modules.used";
            public const string Free = "modules.free";
        }

        public static class Mood
        {
            public const string Reserve = "mood.reserve";
            public const string Income = "mood.income";
        }

        public static class Reforms
        {
            public const string TaxLevel = "reforms.tax_level";
            public const string SocialGuaranteesLevel = "reforms.social_guarantees_level";
        }

        public static class Industries
        {
            public const string Attractiveness = "industries.attractiveness";

            public static class Administrative
            {
                public static class Buildings
                {
                    public const string State = "industries.administrative.buildings.state";
                    public const string Private = "industries.administrative.buildings.private";
                    public const string Total = "industries.administrative.buildings.total";
                }
            }
            
            public static class Mining
            {
                public static class Buildings
                {
                    public const string State = "industries.minning.buildings.state";
                    public const string Private = "industries.minning.buildings.private";
                    public const string Total = "industries.administrative.buildings.total";
                    public const string Available = "industries.minning.buildings.available";
                }
            }

            public static class Production
            {
                public static class Buildings
                {
                    public const string State = "industries.production.buildings.state";
                    public const string Private = "industries.production.buildings.private";
                    public const string Total = "industries.administrative.buildings.total";
                }
            }

            public static class Service
            {
                public static class Buildings
                {
                    public const string State = "industries.service.buildings.state";
                    public const string Private = "industries.service.buildings.private";
                    public const string Total = "industries.administrative.buildings.total";
                    public const string Need = "industries.service.buildings.need";
                }
            }
        }

        public const string Population = "population";

        public static class Flags
        {
            public static class Events
            {
                public const string FirstWedding = "flags.events.first_wedding";
            }
        }

        public static class Counters
        {
            public const string Turns = "counters.turns";
        }

        public static IReadOnlyList<string> MainParameters =>
        [
            Solars.Reserve,
            Solars.Income,
            Mood.Reserve,
            Modules.Used,
            Population
        ];
    }
}
