using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using YAGO.World.Domain.Common;
using YAGO.World.Infrastructure.Database.Colonies;

namespace YAGO.World.Infrastructure.Database.ColonyEvents
{
    public class ColonyEventEntity : IEntity<long>
    {
        public long Id { get; private set; }
        public long ColonyId { get; private set; }
        public string EventCode { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public int TurnNumber { get; private set; }
        public bool IsRead { get; private set; }
        public bool IsCompleted { get; private set; }
        [Timestamp]
        public uint Version { get; private set; }

        public virtual ColonyEntity Colony { get; set; }

        public ColonyEventEntity(
            long id,
            long colonyId,
            string eventCode,
            DateTime createdAtUtc,
            int turnNumber,
            bool isRead,
            bool isCompleted)
        {
            Id = id;
            ColonyId = colonyId;
            EventCode = eventCode;
            TurnNumber = turnNumber;
            CreatedAtUtc = createdAtUtc;
            IsRead = isRead;
            IsCompleted = isCompleted;
        }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<ColonyEventEntity>();
            model.HasKey(m => m.Id);

            model.HasOne(x => x.Colony)
                .WithMany(x => x.Events)
                .HasForeignKey(m => m.ColonyId)
                .OnDelete(DeleteBehavior.Cascade);

            model.HasIndex(m => m.ColonyId);
            model.HasIndex(m => m.IsCompleted);
            model.HasIndex(m => m.TurnNumber);
        }
    }
}
