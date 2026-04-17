using Microsoft.EntityFrameworkCore;
using System;
using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Infrastructure.Database.Colonies;

namespace YAGO.World.Infrastructure.Database.Cycles
{
    public class CycleEntity
    {
        public long Id { get; private set; }
        public long ColonyId { get; private set; }
        [Updatable]
        public DateTime StartAtUtc { get; private set; }
        [Updatable]
        public DateTime? RunAtUtc { get; private set; }
        [Updatable]
        public int StepNumber { get; private set; }
        [Updatable]
        public bool IsComplited { get; private set; }
        [Updatable]
        public string Parameters { get; private set; }
        [Obsolete]
        public CycleState State { get; private set; }

        public virtual ColonyEntity? Colony { get; set; }

        protected CycleEntity() { }

        public CycleEntity(
            long id,
            long colonyId,
            DateTime startAtUtc,
            DateTime? runAtUtc,
            int stepNumber,
            bool isComplited,
            string parameters)
        {
            Id = id;
            ColonyId = colonyId;
            StartAtUtc = startAtUtc;
            RunAtUtc = runAtUtc;
            StepNumber = stepNumber;
            IsComplited = isComplited;
            Parameters = parameters;
        }

        public void UpdateToIsCompleted()
        {
            IsComplited = State == CycleState.Completed;
            State = CycleState.Unknown;
        }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<CycleEntity>();
            model.HasKey(m => m.Id);

            model.HasOne(x => x.Colony)
                .WithMany(x => x.Cycles)
                .HasForeignKey(m => m.ColonyId)
                .OnDelete(DeleteBehavior.Cascade);

            model.HasIndex(m => m.ColonyId);
            model.HasIndex(x => x.RunAtUtc);
        }
    }
}
