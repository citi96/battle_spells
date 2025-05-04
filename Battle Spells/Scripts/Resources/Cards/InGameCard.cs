using Godot;

namespace BattleSpells.Scripts.Resources.Cards
{
    public partial class InGameCard : Control
    {
        [Export] public Control? Card { get; private set; }

        private CardDefinitionBase _cardDefinition = null!;

        private Vector2 _originalScale = Vector2.One;
        private Vector2? _originalPosition;
        private int _originalZIndex = 0;

        public override void _Ready()
        {
            if (Card != null)
                _originalScale = Card.Scale;

            _originalZIndex = ZIndex;
        }

        public void LoadCard(CardDefinitionBase cardDefinition)
        {
            _cardDefinition = cardDefinition;
        }

        public void HiglightCard()
        {
            if (Card == null) return;

            Card.Scale = new Vector2(1.5f, 1.5f);
            Card.Position = new Vector2(0, -Card.Size.Y * 0.5f);
        }

        public void UnHiglightCard()
        {
            if (Card == null) return;

            Card.Scale = _originalScale;
            Card.Position = Vector2.Zero;
        }

        public void OnClick()
        {
            if (Card == null) return;

            if(_originalPosition == null)
                _originalPosition = GlobalPosition;

            ZIndex = 1000;

            var mousePosition = GetViewport().GetMousePosition();
            GlobalPosition = new Vector2(mousePosition.X, mousePosition.Y);
        }

        public void OnRelease()
        {
            //if (CardDefinition == null)
            //    return;
            //if (CardDefinition.CanUseCard())
            //{
            //    CardDefinition.UseCard();
            //}

            if ((_originalPosition != null))
            {
                GlobalPosition = _originalPosition.Value;
                _originalPosition = null;
            }

            ZIndex = _originalZIndex;
        }
    }
}
