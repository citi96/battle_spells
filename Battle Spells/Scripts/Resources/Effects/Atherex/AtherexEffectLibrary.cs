using Godot;

namespace BattleSpells.Scripts.Resources.Effects.Atherex
{
    [GlobalClass]
    public partial class AtherexEffectLibrary : Resource
    {
        public AtherexEffectLibrary()
        {
            CathedralBody = AtherexEffectFactory.CreateVaultPressurePassive();
            CervicalBloom = AtherexEffectFactory.CreateCervicalBloom();
            PlacentalShroud = AtherexEffectFactory.CreatePlacentalShroud();
            BreachCycle = AtherexEffectFactory.CreateBreachCycle();
            CathedralOfUnbirth = AtherexEffectFactory.CreateCathedralOfUnbirth();
            MalformedExplosion = AtherexEffectFactory.CreateMalformedExplosion();
        }

        [Export] public CardEffect CathedralBody { get; set; }
        [Export] public CardEffect CervicalBloom { get; set; }
        [Export] public CardEffect PlacentalShroud { get; set; }
        [Export] public CardEffect BreachCycle { get; set; }
        [Export] public CardEffect CathedralOfUnbirth { get; set; }
        [Export] public CardEffect MalformedExplosion { get; set; }
    }
}
