using YAGO.World.Domain.Cities;

namespace YAGO.World.Host.Controllers.Models.Cities
{
    public class CurrentCityResponse
    {
        public long Id { get; }
        public string Name { get; }
        public long UserId { get; }
        public int Gold { get; }
        public int Population { get; }
        public int Military { get; }
        public int Fortifications { get; }
        public int Control { get; }

        public CurrentCityResponse(City city)
        {
            Id = city.Id;
            Name = city.Name;
            UserId = city.UserId;
            Gold = city.Gold;
            Population = city.Population;
            Military = city.Military;
            Fortifications = city.Fortifications;
            Control = city.Control;
        }
    }
}
