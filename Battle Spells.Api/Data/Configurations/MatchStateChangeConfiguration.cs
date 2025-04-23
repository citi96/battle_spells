using Battle_Spells.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Battle_Spells.Api.Data.Configurations
{
    public class MatchStateChangeConfiguration : IEntityTypeConfiguration<MatchStateChange>
    {
        public void Configure(EntityTypeBuilder<MatchStateChange> builder)
        {
            builder.HasKey(msc => msc.Id);

            builder.HasOne(msc => msc.Match)
               .WithMany()
               .HasForeignKey(msc => msc.MatchId);

            builder.HasOne(msc => msc.Action)
                .WithMany()
                .HasForeignKey(msc => msc.ActionId);
        }
    }
}
