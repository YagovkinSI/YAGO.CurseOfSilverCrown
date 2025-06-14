using YAGO.World.Domain.Constants;

namespace YAGO.World.Domain.Factions
{
    public class Faction
    {
        public int Id { get; }
        public string Name { get; }
        public int Gold { get; }
        public int Investments { get; }
        public int Fortifications { get; }
        public int FortificationCoef => Fortifications / GameSettings.GarrisonPlaceCost;
        public int Size { get; }
        public string UserId { get; }
        public int? SuzerainId { get; }
        public int TurnOfDefeat { get; }

        public Faction(
            int id,
            string name,
            int gold,
            int investments,
            int fortifications,
            int size,
            string userId,
            int? suzerainId,
            int turnOfDefeat)
        {
            Id = id;
            Name = name;
            Gold = gold;
            Investments = investments;
            Fortifications = fortifications;
            Size = size;
            UserId = userId;
            SuzerainId = suzerainId;
            TurnOfDefeat = turnOfDefeat;
        }

        public override string ToString() => Name;
    }
}
