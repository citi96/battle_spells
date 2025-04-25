using Battle_Spells.Models.Enums.Card;

namespace Battle_Spells.Models.DTOs
{
    public record HeroRequest(Guid Id, string Name, int BaseHP, int BaseOrbs, string Description);
    public record CardDto(Guid Id, string Name, string Flavor, int Cost, string EffectDescription, IEnumerable<ECardEffectActivation> EffectActivations, 
        IEnumerable<ECardEffectType> EffectTypes, ECardRarity Rarity, ECardType Type, Guid HeroId);
}
