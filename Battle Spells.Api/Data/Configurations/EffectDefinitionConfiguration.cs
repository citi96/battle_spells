using Battle_Spells.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Battle_Spells.Api.Data.Configurations
{
    public class EffectDefinitionConfiguration : IEntityTypeConfiguration<EffectDefinition>
    {
        public void Configure(EntityTypeBuilder<EffectDefinition> builder)
        {
            builder.HasKey(ed => ed.Id);

            builder.HasOne(ed => ed.ConditionalEffect)
                .WithMany()
                .HasForeignKey(ed => ed.ConditionalEffectId)
                .IsRequired(false);

            builder.HasMany(ed => ed.SubEffects)
                 .WithOne(e => e.ParentEffect)
                 .HasForeignKey(ed => ed.ParentEffectId)
                 .IsRequired(false);
        }
    }
}
