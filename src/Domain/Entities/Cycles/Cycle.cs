using System;
using YAGO.World.Domain.Common.Entities;

namespace YAGO.World.Domain.Entities.Cycles
{
    public class Cycle : IEntity
    {
        private const int TimeoutBetweenCyclesInSeconds = 12;

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
            DateTime? runAtUtc,
            CycleState cycleState)
        {
            Id = id;
            ColonyId = colonyId;
            StepNumber = stepNumber;
            RunAtUtc = runAtUtc;
            State = cycleState;
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

        public bool ReadyForNewCycle()
        {
            return State == CycleState.Completed
                && RunAtUtc < DateTime.UtcNow - TimeSpan.FromSeconds(TimeoutBetweenCyclesInSeconds);
        }
    }
}
