namespace YAGO.World.Host.Models
{
    public class FactionOnList
    {
        public int Id { get; }
        public string Name { get; }
        public int Warriors { get; }
        public int Gold { get; }
        public int Investments { get; }
        public double FortificationCoef { get; }
        public string? Suzerain { get; }
        public int VassalsCount { get; }
        public string? User { get; }

        public FactionOnList(
            int id,
            string name,
            int warriors,
            int gold,
            int investments,
            double fortificationCoef,
            string suzerain,
            int vassalsCount,
            string user)
        {
            Id = id;
            Name = name;
            Warriors = warriors;
            Gold = gold;
            Investments = investments;
            FortificationCoef = fortificationCoef;
            Suzerain = suzerain;
            VassalsCount = vassalsCount;
            User = user;
        }
    }
}
