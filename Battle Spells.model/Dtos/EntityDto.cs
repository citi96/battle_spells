using Battle_Spells.Models.Enums.Card;

namespace Battle_Spells.Models.DTOs
{
    public record HeroRequest(Guid Id, string Name, int BaseHP, int BaseOrbs, string Description);
    public record CardRequest(Guid Id, string Name, string Flavor, int Cost, string EffectDescription, IEnumerable<ECardEffectActivation> Effects, ECardRarity Rarity, ECardType Type, Guid HeroId);
}
