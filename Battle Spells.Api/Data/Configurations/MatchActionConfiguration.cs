using Battle_Spells.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Battle_Spells.Api.Data.Configurations
{
    public class MatchActionConfiguration : IEntityTypeConfiguration<MatchAction>
    {
        public void Configure(EntityTypeBuilder<MatchAction> builder)
        {
            builder.HasKey(ma => ma.Id);

            builder.HasOne(a => a.Match)
                .WithMany(m => m.Actions)
                .HasForeignKey(a => a.MatchId);

            builder.HasOne(a => a.Player)
                .WithMany()
                .HasForeignKey(a => a.PlayerId);

            builder.HasOne(a => a.SourceCard)
                .WithMany()
                .HasForeignKey(a => a.SourceCardId)
                .IsRequired(false);

            builder.HasOne(a => a.TargetCard)
                .WithMany()
                .HasForeignKey(a => a.TargetCardId)
                .IsRequired(false);

            builder.HasMany(ma => ma.ProcessedChanges)
                .WithOne(msc => msc.Action)
                .HasForeignKey(msc => msc.ActionId);
        }
    }
}
