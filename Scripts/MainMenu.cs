using System;
using System.Collections.Generic;
using BattleSpells.Scripts.Managers;
using Godot;

namespace BattleSpells.Scripts
{
    public partial class MainMenu : Control
    {
        [Export] public NodePath HeroSelectionPath;     
        [Export] public NodePath DeckPanelPath;         
        [Export] public NodePath FindMatchButtonPath;   

        private OptionButton _heroOption;
        private Control _deckPanel;
        private Button _findMatchButton;

        // Riferimento al MatchmakingManager (AutoLoad)
        private MatchmakingManager _matchmakingManager;

        public override void _Ready()
        {
            _heroOption = GetNode<OptionButton>(HeroSelectionPath);
            _deckPanel = GetNode<Control>(DeckPanelPath);
            _findMatchButton = GetNode<Button>(FindMatchButtonPath);
            _findMatchButton.Pressed += OnFindMatchPressed;

            // Recupera il MatchmakingManager da AutoLoad (assicurati che il nodo sia registrato come singleton)
            _matchmakingManager = GetNode<MatchmakingManager>("/root/MatchmakingManager");
        }

        private void OnFindMatchPressed()
        {
            // Simula il recupero dell'ID del giocatore (in un vero progetto, questo verrebbe gestito da un PlayerManager)
            Guid playerId = Guid.NewGuid(); // Sostituisci con il vero PlayerId

            // Ottieni l'eroe selezionato: supponiamo che l'OptionButton contenga come items stringhe rappresentanti i GUID degli eroi.
            string heroIdStr = _heroOption.GetItemText(_heroOption.Selected);
            Guid heroId = Guid.Parse(heroIdStr);

            // Recupera la lista del deck selezionato, ad esempio dal pannello deck. Qui, per semplicità, simula il risultato.
            List<Guid> deckCardIds = GetSelectedDeck();

            // Avvia il matchmaking tramite il MatchmakingManager
            _matchmakingManager.StartMatchmaking(playerId, heroId, deckCardIds);

            // Qui, una volta ricevuto il MatchId dal MatchmakingManager (ad esempio tramite un segnale), potrai cambiare la scena.
            GD.Print("Matchmaking avviato, attendi la risposta...");
        }

        // Metodo di esempio per recuperare il deck dalla UI; da personalizzare in base alla configurazione dei controlli.
        private List<Guid> GetSelectedDeck()
        {
            // Potresti scorrere i figli di _deckPanel e raccogliere i GUID delle carte selezionate.
            // Per esempio, se ogni carta è rappresentata da un CheckBox che ha un export "CardId".
            // In questo esempio, restituiamo una lista fittizia.
            return new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        }
    }
}
