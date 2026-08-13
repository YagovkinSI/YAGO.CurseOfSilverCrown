using Microsoft.EntityFrameworkCore;
using System;
using YAGO.World.Infrastructure.Database.Colonies;

namespace YAGO.World.Infrastructure.Database.Turns
{
    public class TurnEntity
    {
        public Guid Id { get; private set; }
        public Guid ColonyId { get; private set; }
        public DateTime StartAtUtc { get; private set; }
        [Updatable]
        public DateTime? RunAtUtc { get; private set; }
        [Updatable]
        public bool IsComplited { get; private set; }
        [Updatable]
        public string JsonData { get; private set; } = "{}";

        public virtual ColonyEntity? Colony { get; set; }

        protected TurnEntity() { }

        public TurnEntity(
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
            JsonData = parameters;
        }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<TurnEntity>();
            model.HasKey(m => m.Id);

            model.HasOne(x => x.Colony)
                .WithMany(x => x.Turns)
                .HasForeignKey(m => m.ColonyId)
                .OnDelete(DeleteBehavior.Cascade);

            model.HasIndex(m => m.ColonyId);
            model.HasIndex(x => x.RunAtUtc);
        }
    }
}
