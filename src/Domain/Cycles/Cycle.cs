using System;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Cycles
{
    public class Cycle : IEntity
    {
        private const int TimeoutBetweenCyclesInMinutes = 2;

        /// <summary>
        /// Идентификатор цикла
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Идентификатор колонии владельца
        /// </summary>
        public long ColonyId { get; }

        /// <summary>
        /// Дата и время запуска цикла
        /// </summary>
        public DateTime CreatedAtUtc { get; private set; }

        /// <summary>
        /// Статус игрового цикла
        /// </summary>
        public CycleStatus Status { get; private set; }

        public DateTime CreateNextCylceAtUtc => CreatedAtUtc + TimeSpan.FromMinutes(TimeoutBetweenCyclesInMinutes);

        public Cycle(
            long id,
            long colonyId,
            DateTime createdAtUtc,
            CycleStatus status)
        {
            Id = id;
            ColonyId = colonyId;
            CreatedAtUtc = createdAtUtc;
            Status = status;
        }

        public static Cycle CreateNew(
            long colonyId)
        {
            return new Cycle(
                id: default,
                colonyId: colonyId,
                createdAtUtc: DateTime.UtcNow,
                status: CycleStatus.Created
            );
        }

        public void SetCompleted()
        {
            if (Status == CycleStatus.Unknown)
                throw new YagoUnknownTypeException(nameof(CycleStatus));

            if (Status == CycleStatus.Completed)
                throw new YagoException("Цикл уже завершён.");

            Status = CycleStatus.Completed;
        }

        public bool IsReadyForNewCycle()
        {
            if (Status == CycleStatus.Unknown)
                throw new YagoUnknownTypeException(nameof(CycleStatus));

            if (Status == CycleStatus.Completed &&
                CreateNextCylceAtUtc <= DateTime.UtcNow)
                return true;

            return false;
        }
    }
}
