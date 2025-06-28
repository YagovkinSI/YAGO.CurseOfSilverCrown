using YAGO.World.Domain.Factions;

namespace YAGO.World.Domain.Units
{
    public class UnitWithFaction
    {
        public int Id { get; }
        public Faction Faction { get; }

        public UnitWithFaction(int id, Faction faction)
        {
            Id = id;
            Faction = faction;
        }
    }
}
