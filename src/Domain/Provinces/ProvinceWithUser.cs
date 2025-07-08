using YAGO.World.Domain.Factions;
using YAGO.World.Domain.Users;

namespace YAGO.World.Domain.Provinces
{
    public class ProvinceWithUser
    {
        public Province Province { get; }
        public Faction Faction { get; }
        public User? User { get; }

        public ProvinceWithUser(Province province, Faction faction, User user)
        {
            Province = province;
            Faction = faction;
            User = user;
        }
    }
}
