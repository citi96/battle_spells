using Battle_Spells.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Battle_Spells.Api.Data.Configurations
{
    public class MatchPlayerCardConfiguration : IEntityTypeConfiguration<MatchPlayerCard>
    {
        public void Configure(EntityTypeBuilder<MatchPlayerCard> builder)
        {
            builder.HasKey(mpc => mpc.Id);

            builder.HasOne(mpc => mpc.Card)
                .WithMany()
                .HasForeignKey(mpc => mpc.CardId);
        }
    }
}
