using Battle_Spells.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Battle_Spells.Api.Data.Configurations
{
    public class PlayerHeroConfiguration : IEntityTypeConfiguration<PlayerHero>
    {
        public void Configure(EntityTypeBuilder<PlayerHero> builder)
        {
            builder.HasKey(pc => new { pc.PlayerId, pc.HeroId });

            builder.HasOne(pc => pc.Player)
                .WithMany(p => p.PlayerHeros)
                .HasForeignKey(pc => pc.PlayerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(pc => pc.Hero)
                .WithOne()
                .HasForeignKey<PlayerHero>(pc => pc.HeroId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
