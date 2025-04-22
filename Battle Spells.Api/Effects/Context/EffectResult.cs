using Battle_Spells.Api.Effects.Interfaces;

namespace Battle_Spells.Api.Effects.Context
{
    public class EffectResult
    {
        public bool Success { get; set; }
        public List<ICardEffect> TriggeredEffects { get; set; } = new();
    }
}
