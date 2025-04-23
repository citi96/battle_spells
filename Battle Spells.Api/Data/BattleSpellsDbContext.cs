using Battle_Spells.Api.Data.Configurations;
using Battle_Spells.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Battle_Spells.Api.Data
{
    public class BattleSpellsDbContext(DbContextOptions<BattleSpellsDbContext> options) : DbContext(options)
    {
        public DbSet<Match> Matches { get; set; } = null!;
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<Hero> Heroes { get; set; } = null!;
        public DbSet<Card> Cards { get; set; } = null!;
        public DbSet<EffectDefinition> EffectDefinitions { get; set; } = null!;
        public DbSet<MatchAction> MatchActions { get; set; } = null!;
        public DbSet<MatchStateChange> MatchStateChanges { get; set; } = null!;
        public DbSet<PlayerCard> PlayerCards { get; set; } = null!;
        public DbSet<MatchPlayerCard> MatchPlayerCards { get; set; } = null!;
        public DbSet<MatchPlayerState> PlayerMatchStates { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CardConfiguration());
            modelBuilder.ApplyConfiguration(new EffectDefinitionConfiguration());
            modelBuilder.ApplyConfiguration(new HeroConfiguration());
            modelBuilder.ApplyConfiguration(new MatchActionConfiguration());
            modelBuilder.ApplyConfiguration(new MatchConfiguration());
            modelBuilder.ApplyConfiguration(new MatchPlayerCardConfiguration());
            modelBuilder.ApplyConfiguration(new MatchPlayerStateConfiguration());
            modelBuilder.ApplyConfiguration(new MatchStateChangeConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerCardConfiguration());
        }
    }
}