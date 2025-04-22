using System;
using Battle_Spells.Models.Enums.Card;
using Godot;
using Godot.Collections;

namespace BattleSpells.Scripts.Resources
{
    [GlobalClass]
    public partial class CardDefinition : Resource
    {
        [Export] public string Name { get; set; } = string.Empty;
        [Export] public string Description { get; set; } = string.Empty;
        [Export] public int Attack { get; set; } = 0;
        [Export] public int Health { get; set; } = 0;
        [Export] public int Cost { get; set; } = 0;
        [Export] public string EffectDescription { get; set; } = string.Empty;
        [Export] public Array<ECardEffectActivation> Effects { get; set; } = [];
        [Export] public ECardRarity Rarity { get; set; } = ECardRarity.Unknown;
        [Export] public ECardType Type { get; set; } = ECardType.Unknown;
        [Export] public ECardButton Button { get; set; } = ECardButton.Unknown;
        [Export] public HeroDefinition HeroDefinition { get; set; }

        public Guid Id { get; set; }
    }
}
