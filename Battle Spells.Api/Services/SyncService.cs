using Battle_Spells.Api.Data;
using Battle_Spells.Api.Entities;
using Battle_Spells.Api.Repositories.Interfaces;
using Battle_Spells.Api.Services.Interfaces;
using Battle_Spells.Models.DTOs;
using Battle_Spells.Models.Enums.Card;
using Microsoft.EntityFrameworkCore;

namespace Battle_Spells.Api.Services
{
    public class SyncService(ICardRepository cardRepository, IHeroRepository heroRepository, IEffectDefinitionRepository effectRepository) : ISyncService
    {
        public async Task<bool> SyncResourcesAsync(SyncResourcesRequest request)
        {
            // Sincronizza gli eroi
            foreach (var heroDto in request.Heroes)
            {
                // Controlla se un eroe con lo stesso HeroName esiste già
                var existingHero = await heroRepository.GetByQueryAsync(h =>
                    h.Name.Equals(heroDto.Name, StringComparison.CurrentCultureIgnoreCase));
                var hero = existingHero.FirstOrDefault();

                if (hero != null)
                {
                    // Aggiorna i campi
                    hero.BaseHP = heroDto.BaseHP;
                    hero.BaseOrbs = heroDto.BaseOrbs;
                    hero.Description = heroDto.Description;
                }
                else
                {
                    // Crea un nuovo eroe
                    hero = new Hero
                    {
                        Id = heroDto.Id,
                        Name = heroDto.Name,
                        BaseHP = heroDto.BaseHP,
                        BaseOrbs = heroDto.BaseOrbs,
                        Description = heroDto.Description
                    };
                }
            }

            // Sincronizza le carte
            foreach (var cardDto in request.Cards)
            {
                // Trova la carta esistente
                var existingCards = await cardRepository.GetByQueryAsync(c =>
                    c.Name.Equals(cardDto.Name, StringComparison.CurrentCultureIgnoreCase));
                var existingCard = existingCards.FirstOrDefault();

                if (existingCard != null)
                {
                    // Aggiorna la carta esistente
                    existingCard.Name = cardDto.Name;
                    existingCard.Flavor = cardDto.Flavor;
                    existingCard.Cost = cardDto.Cost;
                    existingCard.EffectDescription = cardDto.EffectDescription;
                    existingCard.ActivationEffects = cardDto.EffectActivations;
                    existingCard.Rarity = cardDto.Rarity;
                    existingCard.Type = cardDto.Type;
                    existingCard.HeroId = cardDto.HeroId;

                    // Sincronizza gli effetti
                    await SyncCardEffectsAsync(existingCard, cardDto.EffectTypes);
                }
                else
                {
                    // Crea una nuova carta
                    var newCard = new Card
                    {
                        Id = cardDto.Id,
                        Name = cardDto.Name,
                        Flavor = cardDto.Flavor,
                        Cost = cardDto.Cost,
                        EffectDescription = cardDto.EffectDescription,
                        ActivationEffects = cardDto.EffectActivations,
                        Rarity = cardDto.Rarity,
                        Type = cardDto.Type,
                        HeroId = cardDto.HeroId,
                        Effects = []
                    };

                    // Sincronizza gli effetti
                    await SyncCardEffectsAsync(newCard, cardDto.EffectTypes);
                }
            }

            return true;
        }

        private async Task SyncCardEffectsAsync(Card card, IEnumerable<ECardEffectType> effectTypes)
        {
            // Ottieni gli effetti attuali della carta
            var currentEffects = await effectRepository.GetEffectsByCardIdAsync(card.Id);
            var effectsToKeep = new List<Guid>();

            foreach (var effectType in effectTypes)
            {
                // Controlla se esiste già un effetto di questo tipo per questa carta
                var existingEffect = currentEffects.FirstOrDefault(e => e.EffectType == effectType);

                if (existingEffect != null)
                {
                    // Effetto già esistente, mantenere
                    effectsToKeep.Add(existingEffect.Id);
                }
                else
                {
                    // Cerca un effetto riutilizzabile con lo stesso tipo nel database
                    var reusableEffect = await effectRepository.FindReusableEffectByTypeAsync(effectType);

                    if (reusableEffect != null)
                    {
                        // Crea un riferimento specifico per la carta all'effetto riutilizzabile
                        var cardEffect = new EffectDefinition
                        {
                            EffectType = effectType,
                            Amount = reusableEffect.Amount,
                            Description = reusableEffect.Description,
                            SerializedParameters = reusableEffect.SerializedParameters,
                            Condition = reusableEffect.Condition,
                            CardId = card.Id
                        };

                        // Gestire effetti condizionali e sotto-effetti se necessario
                        if (reusableEffect.ConditionalEffectId.HasValue)
                        {
                            // Logica per gli effetti condizionali qui
                        }

                        await effectRepository.AddAsync(cardEffect);
                        effectsToKeep.Add(cardEffect.Id);
                    }
                    else
                    {
                        // Crea una nuova definizione di effetto base con valori predefiniti
                        var newEffect = new EffectDefinition
                        {
                            EffectType = effectType,
                            Amount = 0, // Valore predefinito, da modificare manualmente in seguito
                            Description = $"Default {effectType} effect", // Descrizione predefinita
                            CardId = card.Id
                        };

                        await effectRepository.AddAsync(newEffect);
                        effectsToKeep.Add(newEffect.Id);
                    }
                }
            }

            // Rimuovi gli effetti che non sono più nel DTO
            var effectsToRemove = currentEffects.Where(e => !effectsToKeep.Contains(e.Id));
            foreach (var effectToRemove in effectsToRemove)
            {
                await effectRepository.RemoveAsync(effectToRemove);
            }
        }
    }
}
