using System.Linq;

namespace YAGO.World.Domain.Ships
{
    public static class ShipDataset
    {
        public static Ship[] Get()
        {
            return
            [
                GetDawn(),
                GetResolut()
            ];
        }

        public static Ship GetShip(long shipId)
        {
            return Get().Single(x => x.Id == shipId);
        }

        private static Ship GetDawn()
        {
            return new Ship(
                id: 1,
                "Рассвет-782",
                "Стандартный корабль-город для начинающих правителей. Скромный, но функциональный.",
                contribution: 50000,
                maintenance: 200,
                zones: 140);
        }

        private static Ship GetResolut()
        {
            return new Ship(
                id: 2,
                "Резолют-206",
                "Корабль на три тысячи колонистов, где жители заняты не только добычей ресурсов.",
                contribution: 150000,
                maintenance: 500,
                zones: 450);
        }
    }
}
