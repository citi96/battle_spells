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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Match
            modelBuilder.Entity<Match>()
                .HasOne(m => m.CurrentPlayer)
                .WithMany()
                .HasForeignKey(m => m.CurrentPlayerId)
                .IsRequired(false);

            // Aggiungere la configurazione per Match.Player1 e Match.Player2
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Player1)
                .WithMany()
                .HasForeignKey("Player1Id")
                .IsRequired(false);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Player2)
                .WithMany()
                .HasForeignKey("Player2Id")
                .IsRequired(false);

            // PlayerCard 
            modelBuilder.Entity<PlayerCard>()
                .HasKey(pc => new { pc.PlayerId, pc.CardId });

            modelBuilder.Entity<PlayerCard>()
                .HasOne(pc => pc.Player)
                .WithMany(p => p.PlayerCards)
                .HasForeignKey(pc => pc.PlayerId);

            modelBuilder.Entity<PlayerCard>()
                .HasOne(pc => pc.Card)
                .WithMany(c => c.PlayerCards)
                .HasForeignKey(pc => pc.CardId);

            // EffectDefinition
            modelBuilder.Entity<EffectDefinition>()
                .HasOne(e => e.ConditionalEffect)
                .WithMany()
                .HasForeignKey(e => e.ConditionalEffectId)
                .IsRequired(false);

            modelBuilder.Entity<EffectDefinition>()
                .HasMany(e => e.SubEffects)
                .WithOne()
                .HasForeignKey("ParentEffectId")
                .IsRequired(false);

            // MatchAction
            modelBuilder.Entity<MatchAction>()
                .HasOne(a => a.Match)
                .WithMany(g => g.Actions)
                .HasForeignKey(a => a.MatchId);

            modelBuilder.Entity<MatchAction>()
                .HasOne(a => a.SourceCard)
                .WithMany()
                .HasForeignKey(a => a.SourceCardId)
                .IsRequired(false);

            modelBuilder.Entity<MatchAction>()
                .HasOne(a => a.TargetCard)
                .WithMany()
                .HasForeignKey(a => a.TargetCardId)
                .IsRequired(false);

            // MatchStateChange
            modelBuilder.Entity<MatchStateChange>()
                .HasOne(c => c.Match)
                .WithMany()
                .HasForeignKey(c => c.MatchId);

            modelBuilder.Entity<MatchStateChange>()
                .HasOne(c => c.Action)
                .WithMany()
                .HasForeignKey(c => c.ActionId);

            // MatchPlayerCard 
            modelBuilder.Entity<MatchPlayerCard>()
                .HasOne(mpc => mpc.Card)
                .WithMany()
                .HasForeignKey("CardId");

            // PlayerMatchState 
            modelBuilder.Entity<PlayerMatchState>()
                .HasOne(pms => pms.Player)
                .WithOne()
                .HasForeignKey<PlayerMatchState>("PlayerId");

            modelBuilder.Entity<PlayerMatchState>()
                .HasOne(pms => pms.Hero)
                .WithMany()
                .HasForeignKey("HeroId");

            modelBuilder.Entity<PlayerMatchState>()
                .HasMany(pms => pms.Hand)
                .WithOne()
                .HasForeignKey("PlayerMatchStateHandId")
                .IsRequired(false);

            modelBuilder.Entity<PlayerMatchState>()
                .HasMany(pms => pms.Graveyard)
                .WithOne()
                .HasForeignKey("PlayerMatchStateGraveyardId")
                .IsRequired(false);

            modelBuilder.Entity<PlayerMatchState>()
                .HasMany(pms => pms.Deck.OfType<MatchPlayerCard>())
                .WithOne()
                .HasForeignKey("PlayerMatchStateDeckId")
                .IsRequired(false);

            modelBuilder.Entity<PlayerMatchState>()
                .HasMany(pms => pms.Shop.OfType<MatchPlayerCard>())
                .WithOne()
                .HasForeignKey("PlayerMatchStateShopId")
                .IsRequired(false);

            // Hero
            modelBuilder.Entity<Hero>()
                .HasMany(h => h.Spells)
                .WithOne()
                .HasForeignKey("HeroId")
                .IsRequired(false);

            // Card
            modelBuilder.Entity<Card>()
                .HasMany(c => c.Effects)
                .WithOne()
                .HasForeignKey("CardId")
                .IsRequired(false);
        }
    }
}