using System;
using YAGO.World.Domain.Common.Entities;
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
        /// Шаг цикла
        /// </summary>
        public int StepNumber { get; private set; }

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
        public CycleState State { get; private set; }

        public Cycle(
            long id,
            long colonyId,
            int stepNumber,
            DateTime startAtUtc,
            DateTime? runAtUtc,
            CycleState cycleState)
        {
            Id = id;
            ColonyId = colonyId;
            StepNumber = stepNumber;
            RunAtUtc = runAtUtc;
            State = cycleState;
        }

        public static Cycle CreateNew(
            long colonyId,
            Cycle? prevCycle)
        {
            var startAtUtc = CycleStartDateTimeCalculator.CalcStartAtUtc(prevCycle);
            return new Cycle(
                id: default,
                colonyId: colonyId,
                stepNumber: 0,
                startAtUtc: startAtUtc,
                runAtUtc: null,
                cycleState: CycleState.Ready);
        }

        public void SetInProgress()
        {
            State = CycleState.InProgress;

            if (RunAtUtc == null)
                RunAtUtc = DateTime.UtcNow;
        }

        internal void SetStepNumber(int stepNumber)
        {
            StepNumber = stepNumber;
        }

        internal void SetCompleted()
        {
            State = CycleState.Completed;
        }
    }
}
