using System;
using Battle_Spells.Models.Enums.Card;
using BattleSpells.Scripts.Resources.Effects;
using Godot;
using Godot.Collections;

namespace BattleSpells.Scripts.Resources
{
    public abstract partial class CardDefinitionBase : Resource
    {
        [Export] public string Id;
        [Export] public string Name { get; set; } = string.Empty;
        [Export] public string Flavor { get; set; } = string.Empty;
        [Export] public int Attack { get; set; } = 0;
        [Export] public int Health { get; set; } = 0;
        [Export] public int Cost { get; private set; } = 0;
        [Export] public string EffectDescription { get; set; } = string.Empty;
        [Export] public Array<ECardEffectActivation> EffectActivations { get; set; } = [];
        [Export] public Array<ECardEffectType> EffectTypes { get; set; } = [];
        [Export] public Array<CardEffect> Effects { get; set; } = [];
        [Export] public ECardRarity Rarity { get; set; } = ECardRarity.Unknown;
        [Export] public ECardType Type { get; set; } = ECardType.Unknown;
        [Export] public ECardButton Button { get; set; } = ECardButton.Unknown;
        [Export] public HeroDefinition HeroDefinition { get; set; }

        public IEffectResolver? EffectResolver { get; set; }
        public RuntimeValues RuntimeValues { get; private set; } = new();

        public virtual void InitCard()
        {
            GD.Print($"Card is instantiating");
            RuntimeValues = new RuntimeValues();
        }

        public abstract bool CanUseCard();
        public abstract bool UseCard();

        protected bool ResolveEffects(IEffectResolver resolver)
        {
            return ResolveEffects(new EffectContext(this, resolver));
        }

        protected bool ResolveEffects(EffectContext context)
        {
            if (Effects.Count == 0)
            {
                GD.PushWarning($"{Name} has no effects to resolve.");
                return false;
            }

            foreach (var effect in Effects)
            {
                effect.Resolve(context);
            }

            return true;
        }
    }

    public class RuntimeValues
    {
        private readonly Dictionary<string, int> _counters = new();

        public int GetCounter(string key, int defaultValue = 0) => _counters.TryGetValue(key, out var value) ? value : defaultValue;

        public int IncrementCounter(string key, int increment = 1, int? maxValue = null)
        {
            var current = GetCounter(key);
            current += increment;

            if (maxValue.HasValue)
                current = Math.Clamp(current, 0, maxValue.Value);

            _counters[key] = current;
            return current;
        }

        public void SetCounter(string key, int value)
        {
            _counters[key] = value;
        }
    }
}
