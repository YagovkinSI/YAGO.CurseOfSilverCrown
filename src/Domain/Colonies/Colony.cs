using YAGO.World.Domain.Common.Entities;

namespace YAGO.World.Domain.Colonies
{
    /// <summary>
    /// Колония
    /// </summary>
    public class Colony : IEntity
    {
        /// <summary>
        /// Идентифиикатор колонии
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Идентифиикатор пользователя владельца
        /// </summary>
        public long UserId { get; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Солары
        /// </summary>
        public decimal Solars { get; }

        /// <summary>
        /// Доход соларов
        /// </summary>
        public decimal SolarsIncome { get; }

        /// <summary>
        /// Репутация
        /// </summary>
        public decimal Reputation { get; }

        /// <summary>
        /// Население
        /// </summary>
        public int Population { get; }

        /// <summary>
        /// Площадей занято
        /// </summary>
        public int ZonesOccupied { get; }

        /// <summary>
        /// Площадей всего
        /// </summary>
        public int ZonesTotal { get; }

        public Colony(
            long id, 
            long userId, 
            string name, 
            decimal solars, 
            decimal solarsIncome, 
            decimal reputation, 
            int population, 
            int zonesOccupied, 
            int zonesTotal)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Solars = solars;
            SolarsIncome = solarsIncome;
            Reputation = reputation;
            Population = population;
            ZonesOccupied = zonesOccupied;
            ZonesTotal = zonesTotal;
        }
    }
}
