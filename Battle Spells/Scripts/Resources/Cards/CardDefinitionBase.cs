using System;
using Battle_Spells.Models.Enums.Card;
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
        [Export] public ECardRarity Rarity { get; set; } = ECardRarity.Unknown;
        [Export] public ECardType Type { get; set; } = ECardType.Unknown;
        [Export] public ECardButton Button { get; set; } = ECardButton.Unknown;
        [Export] public HeroDefinition HeroDefinition { get; set; }

        public RuntimeValues RuntimeValues { get; private set; }

        public virtual void InitCard()
        {
            GD.Print($"Card is instantiating");
            RuntimeValues = new RuntimeValues();
        }

        public abstract bool CanUseCard();
        public abstract bool UseCard();
    }

    public class RuntimeValues
    {
    }
}
