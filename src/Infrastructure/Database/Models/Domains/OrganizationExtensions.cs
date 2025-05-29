namespace YAGO.World.Infrastructure.Database.Models.Domains
{
    public static class OrganizationExtensions
    {
        public static Domain.Organizations.Organization ToDomain(this Organization organization)
        {
            if (organization == null)
                return null;

            return new Domain.Organizations.Organization(
                organization.Id,
                organization.Gold
                );
        }
    }
}
