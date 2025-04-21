using Godot;
using System;

namespace BattleSpells.Scripts.Resources
{
    [GlobalClass]
    public partial class HeroDefinition : Resource
    {
        [Export] public string HeroName { get; set; } = "";
        [Export] public int BaseHP { get; set; } = 30;
        [Export] public int BaseOrbs { get; set; } = 3;
        [Export] public string Description { get; set; } = "";

        public Guid Id { get; set; }

    }
}