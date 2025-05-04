using Godot;

namespace BattleSpells.Scripts.Resources.Cards
{
    [GlobalClass]
    public partial class MinionCardDefinition : CardDefinitionBase
    {
        public override bool CanUseCard()
        {
            //Add logic
            return true;
        }

        public override bool UseCard()
        {
            throw new System.NotImplementedException();
        }
    }
}
