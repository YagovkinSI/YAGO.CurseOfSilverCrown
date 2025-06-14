using YAGO.World.Domain.Factions.Enums;

namespace YAGO.World.Domain.Factions.Extensions
{
    public static class FactionOrderByExtension
    {
        public static FactionOrderBy ToFactionOrderBy(this string source)
        {
            return source switch
            {
                "name" => FactionOrderBy.Name,
                "warriorCount" => FactionOrderBy.WarriorCount,
                "gold" => FactionOrderBy.Gold,
                "investments" => FactionOrderBy.Investments,
                "fortifications" => FactionOrderBy.Fortifications,
                "suzerain" => FactionOrderBy.Suzerain,
                "user" => FactionOrderBy.User,
                _ => FactionOrderBy.VassalCount,
            };
        }
    }
}
