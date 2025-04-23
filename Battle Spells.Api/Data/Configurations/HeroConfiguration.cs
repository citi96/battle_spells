using Battle_Spells.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Battle_Spells.Api.Data.Configurations
{
    public class HeroConfiguration : IEntityTypeConfiguration<Hero>
    {
        public void Configure(EntityTypeBuilder<Hero> builder)
        {
            builder.HasKey(h => h.Id);

            builder.HasMany(h => h.Spells)
                .WithOne()
                .HasForeignKey(h => h.HeroId)
                .IsRequired(false);
        }
    }
}
