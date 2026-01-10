using Microsoft.EntityFrameworkCore;
using System;
using YAGO.World.Domain.Cycles;
using YAGO.World.Infrastructure.Database.Colonies;

namespace YAGO.World.Infrastructure.Database.Cycles
{
    public class CycleEntity
    {
        public long Id { get; private set; }
        public long ColonyId { get; private set; }
        public int StepNumber { get; private set; }
        public DateTime? RunAtUtc { get; private set; }
        public CycleState State { get; private set; }

        [Obsolete]
        public DateTime? CompletedUtc { get; private set; }

        public virtual ColonyEntity? Colony { get; set; }

        protected CycleEntity() { }

        public CycleEntity(
            long id,
            long colonyId,
            int stepNumber,
            DateTime? runAtUtc,
            CycleState state)
        {
            Id = id;
            ColonyId = colonyId;
            StepNumber = stepNumber;
            RunAtUtc = runAtUtc;
            State = state;
        }

        public void Migrate()
        {
            if (CompletedUtc.HasValue)
            {

                StepNumber = 4;
                RunAtUtc = CompletedUtc;
                State = CycleState.Completed;
            }
            else
            {
                StepNumber = 0;
                RunAtUtc = null;
                State = CycleState.Ready;
            }
        }

        internal static CycleEntity CreateNew(
            long colonyId)
        {
            return new CycleEntity(
                id: default,
                colonyId: colonyId,
                stepNumber: 0,
                runAtUtc: null,
                state: CycleState.Ready
            );
        }

        internal void Update(Cycle cycle)
        {
            StepNumber = cycle.StepNumber;
            RunAtUtc = cycle.RunAtUtc;
            State = cycle.State;
        }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<CycleEntity>();
            model.HasKey(m => m.Id);

            model.HasOne(x => x.Colony).
                WithMany(x => x.Cycles).
                HasForeignKey(m => m.ColonyId);

            model.HasIndex(m => m.ColonyId);
            model.HasIndex(x => x.RunAtUtc);
        }
    }
}
