using System;
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
        /// Текущее событие
        /// </summary>
        public string? ActiveEventId { get; private set; }

        /// <summary>
        /// Шаг цикла
        /// </summary>
        public int StepNumber { get; private set; }

        /// <summary>
        /// Статус цикла
        /// </summary>
        public bool IsComplited { get; private set; }

        public CycleResult PreviousCycleResult { get; }

        public Cycle(
            Guid id,
            Guid colonyId,
            DateTime startAtUtc,
            DateTime? runAtUtc,
            string? activeEventId,
            int stepNumber,
            bool isComplited,
            CycleResult previousCycleResult)
        {
            Id = id;
            ColonyId = colonyId;
            StartAtUtc = startAtUtc;
            RunAtUtc = runAtUtc;
            ActiveEventId = activeEventId;
            StepNumber = stepNumber;
            IsComplited = isComplited;
            PreviousCycleResult = previousCycleResult;
        }

        public static Cycle CreateNew(
            Guid colonyId,
            Cycle? prevCycle,
            CycleResult previousCycleResult)
        {
            var startAtUtc = CycleStartDateTimeCalculator.CalcStartAtUtc(prevCycle);

            return new Cycle(
                id: Guid.NewGuid(),
                colonyId: colonyId,
                startAtUtc: startAtUtc,
                runAtUtc: null,
                activeEventId: null,
                stepNumber: 0,
                isComplited: false,
                previousCycleResult: previousCycleResult);
        }

        public void SetStepNumber(int stepNumber, string? activeEvent, bool isCycleEnded)
        {
            StepNumber = stepNumber;
            ActiveEventId = activeEvent;
            if (isCycleEnded)
                IsComplited = true;
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
    }
}
