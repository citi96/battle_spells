using Battle_Spells.Models.Enums.Card;

namespace Battle_Spells.Api.Entities
{
    public class Card
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Flavor { get; set; } = string.Empty;
        public int MaxHealth { get; set; }
        public int ManaCost { get; set; }
        public int Attack { get; set; }
        public int Cost { get; set; }
        public string EffectDescription { get; set; } = string.Empty;

        private string _activationEffectsData = string.Empty;
        public IEnumerable<ECardEffectActivation> ActivationEffects
        {
            get => string.IsNullOrEmpty(_activationEffectsData)
                ? []
                : _activationEffectsData.Split(',').Select(e => Enum.Parse<ECardEffectActivation>(e));
            set => _activationEffectsData = string.Join(",", value.Select(e => e.ToString()));
        }

        public string ActivationEffectsData
        {
            get => _activationEffectsData;
            set => _activationEffectsData = value;
        }

        public ECardRarity Rarity { get; set; } = ECardRarity.Unknown;
        public ECardType Type { get; set; } = ECardType.Unknown;
        public Guid? HeroId { get; set; }
        public virtual ICollection<EffectDefinition> Effects { get; set; } = [];
    }
}
