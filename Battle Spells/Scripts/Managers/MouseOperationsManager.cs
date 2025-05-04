using BattleSpells.Scripts.Resources.Cards;
using Godot;

namespace BattleSpells.Scripts.Managers
{
    public partial class MouseOperationsManager : Control
    {
        InGameCard? _currentCard = null;
        bool _leftButtonPressed = false;

        public override void _Ready()
        {
            RegisterUIElements(this);
        }

        public override void _Input(InputEvent @event)
        {
            if (_leftButtonPressed)
                HandleMouseClick();
        }

        private void RegisterUIElements(Node parent)
        {
            foreach (var child in parent.GetChildren())
            {
                if (child is Control control)
                {
                    control.GuiInput += (InputEvent @event) => OnControlInput(control, @event);

                    control.MouseEntered += () => OnMouseEntered(control);
                    control.MouseExited += () => OnMouseExited(control);

                }

                // Registra ricorsivamente anche i controlli figli
                RegisterUIElements(child);
            }
        }

        private void OnControlInput(Control control, InputEvent @event)
        {
            if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left)
            {
                if (mouseEvent.Pressed)
                {
                    _leftButtonPressed = true;
                    HandleMouseClick();
                    GD.Print("mouse pressed");
                }
                else
                {
                    _leftButtonPressed = false;
                    HandleMouseRelease();
                    GD.Print("mouse released");
                }
            }
        }

        private void HandleMouseClick()
        {
            if (_currentCard == null)
                return;

            _currentCard.OnClick();
        }

        private void HandleMouseRelease()
        {
            if (_currentCard == null)
                return;

            _currentCard.OnRelease();
        }

        private void OnMouseEntered(Control control)
        {
            HandleCardDetection(control);
        }

        private void HandleCardDetection(Control control)
        {
            if (_leftButtonPressed)
                return;

            var cardInstance = control.GetParentOrNull<InGameCard>();
            if (cardInstance != null && cardInstance != _currentCard)
            {
                _currentCard?.UnHiglightCard();

                _currentCard = cardInstance;
                _currentCard.HiglightCard();
            }
        }

        private void OnMouseExited(Control control)
        {
            if (_leftButtonPressed)
                return;

            _currentCard?.UnHiglightCard();
        }
    }
}
