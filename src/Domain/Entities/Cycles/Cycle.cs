using System;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Services;

namespace YAGO.World.Domain.Entities.Cycles
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
        /// Дата и время начала цикла (раньше запусить нельзя)
        /// </summary>
        public DateTime StartAtUtc { get; private set; }

        /// <summary>
        /// Дата и время запуска цикла
        /// </summary>
        public DateTime? RunAtUtc { get; private set; }

        /// <summary>
        /// Шаг цикла
        /// </summary>
        public int StepNumber { get; private set; }

        /// <summary>
        /// Статус цикла
        /// </summary>
        public bool IsComplited { get; private set; }

        public Cycle(
            long id,
            long colonyId,
            DateTime startAtUtc,
            DateTime? runAtUtc,
            int stepNumber,
            bool isComplited)
        {
            Id = id;
            ColonyId = colonyId;
            StartAtUtc = startAtUtc;
            RunAtUtc = runAtUtc;
            StepNumber = stepNumber;
            IsComplited = isComplited;
        }

        public static Cycle CreateNew(
            long colonyId,
            Cycle? prevCycle)
        {
            var startAtUtc = CycleStartDateTimeCalculator.CalcStartAtUtc(prevCycle);
            return new Cycle(
                id: default,
                colonyId: colonyId,
                startAtUtc: startAtUtc,
                runAtUtc: null,
                stepNumber: 0,
                isComplited: false);
        }

        public CycleState GetState()
        {
            if (IsComplited) 
                return CycleState.Completed;

            if (RunAtUtc != null)
                return CycleState.InProgress;

            return CycleState.Ready;
        }

        public void SetStepNumber(int stepNumber, bool isCycleEnded)
        {
            StepNumber = stepNumber;
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
