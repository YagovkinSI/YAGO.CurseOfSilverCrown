using System.Collections.Generic;
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
        public int Solars { get; private set; }

        /// <summary>
        /// Идентифиикатор корабля
        /// </summary>
        public long ShipId => 1;

        /// <summary>
        /// Установленные законы
        /// </summary>
        public GavernorType StartGavernorType { get; }

        /// <summary>
        /// Контракты колонии
        /// </summary>
        public Dictionary<long, int> Contracts { get; private set; }


        public Colony(
            long id,
            long userId,
            string name,
            int solars,
            GavernorType startGavernorType,
            Dictionary<long, int> contracts)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Solars = solars;
            StartGavernorType = startGavernorType;
            Contracts = contracts;
        }

        public static Colony CreateNew(
            long userId,
            string name,
            GavernorType gavernorType)
        {
            return new Colony(
                id: default,
                userId: userId,
                name: name,
                solars: 1000,
                startGavernorType: gavernorType,
                contracts: []
            );
        }

        public void AddSolars(int value)
        {
            Solars += value;
        }

        public void AddContract(long contractId)
        {
            if (Contracts.ContainsKey(contractId))
                Contracts[contractId]++;
            else
                Contracts.Add(contractId, 1);
        }
    }
}
