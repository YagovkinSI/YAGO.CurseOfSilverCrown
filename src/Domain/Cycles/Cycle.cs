using System;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Cycles
{
    public class Cycle : IEntity
    {
        /// <summary>
        /// Идентификатор цикла
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Идентификатор колонии владельца
        /// </summary>
        public long ColonyId { get; }

        /// <summary>
        /// Дата и время завершения цикла
        /// </summary>
        public DateTime? CompletedUtc { get; private set; }

        public Cycle(
            long id,
            long colonyId,
            DateTime? completedUtc)
        {
            Id = id;
            ColonyId = colonyId;
            CompletedUtc = completedUtc;
        }

        public void SetCompleted()
        {
            if (CompletedUtc != null)
                throw new YagoException("Цикл уже завершён, необходимо дождаться нового цикла.");

            CompletedUtc = DateTime.UtcNow;
        }
    }
}
