using System;
using System.Collections.Generic;
using Battle_Spells.model.Enums.Hub;
using Battle_Spells.model.Enums.Match;
using BattleSpells.Scripts.Helper;
using BattleSpells.Scripts.Holders;
using BattleSpells.Scripts.Resources.Cards;
using Godot;

namespace BattleSpells.Scripts.Managers
{
    public partial class MatchManager : Node
    {
        [Export] public string RestEndpoint { get; private set; } = "/match";
        [Export] public PackedScene CardScene { get; private set; } = null!;
        [Export] public PlayerHolder Player1 { get; private set; } = null!;
        [Export] public PlayerHolder Player2 { get; private set; } = null!;

        public PlayerHolder CurrentPlayer { get; private set; } = null!;

        public static MatchManager Instance { get; private set; } = null!;

        private static ResourcesManager? _resourcesManager;
        public static ResourcesManager ResourcesManager
        {
            get
            {
                if(_resourcesManager == null)
                {
                    _resourcesManager = ResourceLoader.Load<ResourcesManager>("res://Resources/ResourceManager.tres");
                    _resourcesManager.Init();
                }

                return _resourcesManager;
            }
        }

        public override void _Ready()
        {
            if (Instance != null && Instance != this)
            {
                GD.PushError("Multiple instances of GameManager detected!");
                QueueFree();
                return;
            }

            Instance = this;
            GD.Print("GameManager ready");



            MessageDispatcher.Instance.RegisterHandler(EHubMessageType.PickNewCard, HandlePlayerPicksNewCard);
        }

        private void InitPlayers()
        {
            Player1.Init(this);
            Player2.Init(this);
        }

        public void MatchOperationFromPlayer(MatchOperation matchOperation)
        {
            var player = CurrentPlayer;
             switch (matchOperation)
            {
                default:
                    GD.PushError("Unknown match operation: " + matchOperation);
                    break;
            }
        }

        #region SignalR Message Handling

        private void HandlePlayerPicksNewCard(Dictionary<string, object> messageData)
        {
            if (messageData.TryGetValue("cardId", out object? value))
                return;

            if (value == null)
                return;

            var cardId = (Guid)value;
            GD.Print($"Carta pescata: {cardId}");

            var card = ResourcesManager.GetInstanceOfACard(cardId);
            if (card == null)
            {
                GD.PushError($"Card {cardId} not found in ResourcesManager.");
                return;
            }

            var inGameCard = CardScene.Instantiate<InGameCard>();
            inGameCard.LoadCard(card);

            CurrentPlayer.HandGrid.AddChild(inGameCard);

            //EmitSignal(SignalName.MatchStarted, matchId.ToString());
        }

        #endregion

    }
}
