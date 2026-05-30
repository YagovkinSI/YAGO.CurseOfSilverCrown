using System;
using System.Collections.Generic;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Services;

namespace YAGO.World.Domain.Entities.Cycles
{
    public class Cycle : IEntity<Guid>
    {
        /// <summary>
        /// Идентификатор цикла
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Идентификатор колонии владельца
        /// </summary>
        public Guid ColonyId { get; }

        /// <summary>
        /// Дата и время начала цикла (раньше запусить нельзя)
        /// </summary>
        public DateTime StartAtUtc { get; private set; }

        /// <summary>
        /// Дата и время запуска цикла
        /// </summary>
        public DateTime? RunAtUtc { get; private set; }

        /// <summary>
        /// Статус цикла
        /// </summary>
        public bool IsComplited { get; private set; }

        public IReadOnlyList<string> GameEventsIds { get; }

        public Cycle(
            Guid id,
            Guid colonyId,
            DateTime startAtUtc,
            DateTime? runAtUtc,
            bool isComplited,
            IReadOnlyList<string> gameEventsIds)
        {
            Id = id;
            ColonyId = colonyId;
            StartAtUtc = startAtUtc;
            RunAtUtc = runAtUtc;
            IsComplited = isComplited;
            GameEventsIds = gameEventsIds;
        }

        public static Cycle CreateNew(
            Guid colonyId,
            Cycle? prevCycle,
            IReadOnlyList<string> gameEventsIds)
        {
            var startAtUtc = CycleStartDateTimeCalculator.CalcStartAtUtc(prevCycle);
            return new Cycle(
                id: Guid.NewGuid(),
                colonyId: colonyId,
                startAtUtc: startAtUtc,
                runAtUtc: null,
                isComplited: false,
                gameEventsIds);
        }

        public void RunCycle()
        {
            if (IsComplited)
                throw new YagoException("Цикл уже завершен.");
            if (StartAtUtc > DateTime.UtcNow)
                throw new YagoException("Цикл не готов к запуску. Дождитесь готовности не более двух минут.");

            if (RunAtUtc == null)
                RunAtUtc = DateTime.UtcNow;
        }

        public void SetCompleted()
        {
            IsComplited = true;
        }
    }
}
