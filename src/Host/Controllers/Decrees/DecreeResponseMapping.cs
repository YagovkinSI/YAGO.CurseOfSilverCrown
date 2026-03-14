using YAGO.World.Domain.Entities.Decrees;

namespace YAGO.World.Host.Controllers.Decrees
{
    public static class DecreeResponseMapping
    {
        public static DecreeDetails ToMyDataResponse(
            this Decree source)
        {
            return new DecreeDetails(
                source.Id,
                source.Name,
                source.Image,
                source.Text,
                source.Parameters,
                source.Description);
        }
    }
}
