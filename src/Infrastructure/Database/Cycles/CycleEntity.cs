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
        public DateTime CreatedAtUtc { get; private set; }
        public CycleStatus Status { get; private set; }

        [Obsolete("Теперь используется Status и CreatedAtUtc")]
        public DateTime? CompletedUtc { get; private set; }


        public virtual ColonyEntity? Colony { get; set; }

        protected CycleEntity() { }

        public CycleEntity(
            long id,
            long colonyId,
            DateTime createdAtUtc,
            CycleStatus status)
        {
            Id = id;
            ColonyId = colonyId;
            CreatedAtUtc = createdAtUtc;
            Status = status;
        }

        internal void Update(Cycle cycle)
        {
            Status = cycle.Status;
        }

        [Obsolete]
        internal static void MoveToStatus(CycleEntity cycle)
        {
            if (cycle.CompletedUtc != null)
            {
                cycle.CreatedAtUtc = cycle.CompletedUtc.Value;
                cycle.Status = CycleStatus.Completed;
            }
            else
            {
                cycle.CreatedAtUtc = DateTime.UtcNow;
                cycle.Status = CycleStatus.Created;
            }
        }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<CycleEntity>();
            model.HasKey(m => m.Id);

            model.HasOne(x => x.Colony).
                WithMany(x => x.Cycles).
                HasForeignKey(m => m.ColonyId);

            model.HasIndex(m => m.ColonyId);
            model.HasIndex(x => x.CreatedAtUtc);
        }
    }
}
