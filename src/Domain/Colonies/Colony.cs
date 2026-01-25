using System;
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
        public long ShipId { get; private set; }

        /// <summary>
        /// Установленные законы
        /// </summary>
        public GavernorType CodeOfLaws { get; }

        /// <summary>
        /// Контракты колонии
        /// </summary>
        public Dictionary<long, int> Contracts { get; private set; }

        /// <summary>
        /// Флаг деактивации колонии игроком
        /// </summary>
        public bool Deactivated { get; private set; }

        /// <summary>
        /// Время деактивации колонии игроком
        /// </summary>
        public DateTime? DeactivateAtUtc { get; private set; }


        public Colony(
            long id,
            long userId,
            string name,
            int solars,
            long shipId,
            GavernorType startGavernorType,
            Dictionary<long, int> contracts,
            bool deactivated,
            DateTime? deactivateAtUtc)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Solars = solars;
            ShipId = shipId;
            CodeOfLaws = startGavernorType;
            Contracts = contracts;
            Deactivated = deactivated;
            DeactivateAtUtc = deactivateAtUtc;
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
                shipId: 1,
                startGavernorType: gavernorType,
                contracts: [],
                deactivated: false,
                deactivateAtUtc: null
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

        public void SetShip(int shipId)
        {
            ShipId = shipId;
        }

        public void Deactivate()
        {
            Deactivated = true;
            DeactivateAtUtc = DateTime.UtcNow;
        }
    }
}
