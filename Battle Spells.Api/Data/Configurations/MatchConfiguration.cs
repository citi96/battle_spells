using Battle_Spells.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Battle_Spells.Api.Data.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.HasKey(m => m.Id);

            builder.HasOne(m => m.CurrentPlayer)
                .WithMany()
                .HasForeignKey(m => m.CurrentPlayerId)
                .IsRequired(false);

            builder.HasOne(m => m.Player1)
                .WithMany()
                .HasForeignKey(m => m.Player1Id)
                .IsRequired(false);

            builder.HasOne(m => m.Player2)
                .WithMany()
                .HasForeignKey(m => m.Player2Id)
                .IsRequired(false);

            builder.HasMany(m => m.Actions)
                .WithOne(ma => ma.Match)
                .HasForeignKey(ma => ma.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Player1MatchState)
                .WithOne()
                .HasForeignKey<Match>(m => m.Player1MatchStateId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Player2MatchState)
                .WithOne()
                .HasForeignKey<Match>(m => m.Player2MatchStateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
