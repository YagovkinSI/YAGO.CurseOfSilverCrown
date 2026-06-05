using Microsoft.EntityFrameworkCore;
using System;
using YAGO.World.Infrastructure.Database.Colonies;

namespace YAGO.World.Infrastructure.Database.Cycles
{
    public class CycleEntity
    {
        public Guid Id { get; private set; }
        public Guid ColonyId { get; private set; }
        [Updatable]
        public DateTime StartAtUtc { get; private set; }
        [Updatable]
        public DateTime? RunAtUtc { get; private set; }
        [Obsolete]
        public int StepNumber { get; private set; }
        [Updatable]
        public bool IsComplited { get; private set; }
        [Updatable]
        public string Parameters { get; private set; }

        public virtual ColonyEntity? Colony { get; set; }

        protected CycleEntity() { }

        public CycleEntity(
            Guid id,
            Guid colonyId,
            DateTime startAtUtc,
            DateTime? runAtUtc,
            bool isComplited,
            string parameters)
        {
            Id = id;
            ColonyId = colonyId;
            StartAtUtc = startAtUtc;
            RunAtUtc = runAtUtc;
            IsComplited = isComplited;
            Parameters = parameters;
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
