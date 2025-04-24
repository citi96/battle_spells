using Battle_Spells.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Battle_Spells.Api.Data.Configurations
{
    public class MatchPlayerStateConfiguration : IEntityTypeConfiguration<MatchPlayerState>
    {
        public void Configure(EntityTypeBuilder<MatchPlayerState> builder)
        {
            builder.HasKey(mps => mps.Id);

            builder.HasOne(mps => mps.Player)
                .WithOne()
                .HasForeignKey<MatchPlayerState>(mps => mps.PlayerId);

            builder.HasOne(mps => mps.Hero)
                .WithMany()
                .HasForeignKey(mps => mps.HeroId);

            builder.HasMany(mps => mps.Hand)
                .WithOne()
                .HasForeignKey(mps => mps.PlayerMatchStateHandId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(mps => mps.Graveyard)
                .WithOne()
                .HasForeignKey(mps => mps.PlayerMatchStateGraveyardId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(mps => mps.Deck)
                .WithOne()
                .HasForeignKey(mpc => mpc.PlayerMatchStateDeckId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(mps => mps.Shop)
                .WithOne()
                .HasForeignKey(mpc => mpc.PlayerMatchStateShopId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
