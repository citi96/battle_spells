using Battle_Spells.Api.Data;
using Battle_Spells.Api.Entities;
using Battle_Spells.Api.Services.Interfaces;
using Battle_Spells.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Battle_Spells.Api.Services
{
    public class SyncService(BattleSpellsDbContext dbContext) : ISyncService
    {
        public async Task<bool> SyncResourcesAsync(SyncResourcesRequest request)
        {
            // Sincronizza gli eroi
            foreach (var heroDto in request.Heroes)
            {
                // Controlla se un eroe con lo stesso HeroName esiste già
                var existingHero = await dbContext.Heroes.FirstOrDefaultAsync(h => h.Name.Equals(heroDto.Name, StringComparison.CurrentCultureIgnoreCase));
                if (existingHero != null)
                {
                    // Aggiorna i campi (oppure ignora, a seconda della logica di business)
                    existingHero.BaseHP = heroDto.BaseHP;
                    existingHero.BaseOrbs = heroDto.BaseOrbs;
                    existingHero.Description = heroDto.Description;
                    
                    dbContext.Heroes.Update(existingHero);
                }
                else
                {
                    var newHero = new Hero
                    {
                        Name = heroDto.Name,
                        BaseHP = heroDto.BaseHP,
                        BaseOrbs = heroDto.BaseOrbs,
                        Description = heroDto.Description
                    };
                    await dbContext.Heroes.AddAsync(newHero);
                }
            }

            // Sincronizza le carte
            foreach (var cardDto in request.Cards)
            {
                var existingCard = await dbContext.Cards.FirstOrDefaultAsync(c => c.Name.Equals(cardDto.Name, StringComparison.CurrentCultureIgnoreCase));
                if (existingCard != null)
                {
                    existingCard.Name = cardDto.Name;
                    existingCard.Flavor = cardDto.Flavor;
                    existingCard.ManaCost = cardDto.Cost;
                    existingCard.ActivationEffectsData = string.Join(",", cardDto.Effects.Select(e => e.ToString()));
                    existingCard.EffectDescription = cardDto.EffectDescription;
                    existingCard.Rarity = cardDto.Rarity;
                    existingCard.Type = cardDto.Type;
                    existingCard.HeroId = cardDto.HeroId;

                    dbContext.Cards.Update(existingCard);
                }
                else
                {
                    // Crea una nuova carta
                    var newCard = new Card
                    {
                        Name = cardDto.Name,
                        Flavor = cardDto.Flavor,
                        Cost = cardDto.Cost,
                        EffectDescription = cardDto.EffectDescription,
                        ActivationEffects = cardDto.Effects,
                        Rarity = cardDto.Rarity,
                        Type = cardDto.Type,
                        HeroId = cardDto.HeroId
                    };

                    await dbContext.Cards.AddAsync(newCard);
                }
            }

            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
