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
        public DateTime? CompletedUtc { get; private set; }

        public virtual ColonyEntity? Colony { get; set; }

        protected CycleEntity() { }

        public CycleEntity(
            long id,
            long colonyId,
            DateTime? completedUtc)
        {
            Id = id;
            ColonyId = colonyId;
            CompletedUtc = completedUtc;
        }

        internal static CycleEntity CreateNew(
            long colonyId)
        {
            return new CycleEntity(
                id: default,
                colonyId: colonyId,
                completedUtc: null
            );
        }

        internal void Update(Cycle cycle)
        {
            CompletedUtc = cycle.CompletedUtc;
        }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<CycleEntity>();
            model.HasKey(m => m.Id);

            model.HasOne(x => x.Colony).
                WithMany(x => x.Cycles).
                HasForeignKey(m => m.ColonyId);

            model.HasIndex(m => m.ColonyId);
            model.HasIndex(x => x.CompletedUtc);
        }
    }
}
