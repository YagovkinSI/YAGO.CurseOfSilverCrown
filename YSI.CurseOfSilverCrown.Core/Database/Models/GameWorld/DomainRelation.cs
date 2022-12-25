using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld
{
    public class DomainRelation
    {
        public int Id { get; set; }

        public int SourceDomainId { get; set; }

        [Display(Name = "Владение")]
        public int TargetDomainId { get; set; }

        [Display(Name = "Включая его вассалов?")]
        public bool IsIncludeVassals { get; set; }

        [Display(Name = "Разрешение на право прохода по своей территории")]
        public bool PermissionOfPassage { get; set; }

        public virtual Domain SourceDomain { get; set; }

        public virtual Domain TargetDomain { get; set; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<DomainRelation>();
            model.HasKey(m => m.Id);

            model.HasOne(m => m.SourceDomain)
                .WithMany(m => m.Relations)
                .HasForeignKey(m => m.SourceDomainId)
                .OnDelete(DeleteBehavior.Restrict);
            model.HasOne(m => m.TargetDomain)
                .WithMany(m => m.RelationsToThisDomain)
                .HasForeignKey(m => m.TargetDomainId)
                .OnDelete(DeleteBehavior.Restrict);

            model.HasIndex(m => m.SourceDomainId);
            model.HasIndex(m => m.TargetDomainId);
            model.HasIndex(m => new { m.SourceDomainId, m.TargetDomainId }).IsUnique();
        }
    }
}
