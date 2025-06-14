namespace YAGO.World.Infrastructure.Database.Models.Domains
{
    public static class OrganizationExtensions
    {
        public static Domain.Factions.Faction ToDomain(this Organization source)
        {
            if (source == null)
                return null;

            return new Domain.Factions.Faction(
                source.Id,
                source.Name,
                source.Gold,
                source.Investments,
                source.Fortifications,
                source.Size,
                source.UserId,
                source.SuzerainId,
                source.TurnOfDefeat
            );
        }
    }
}
