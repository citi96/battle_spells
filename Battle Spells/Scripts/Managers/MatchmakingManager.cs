using Battle_Spells.model.Enums.Hub;
using Battle_Spells.model.Enums.Matchmaking;
using Battle_Spells.Models.Enums.Match;
using Godot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BattleSpells.Scripts.Managers
{
    /// <summary>
    /// Questo nodo gestisce il flusso di matchmaking: Find Match, Hero Selection, Deck Selection, Start match.
    /// </summary>
    public partial class MatchmakingManager : Node
    {
        [Export] public string RestApiUrl = "http://localhost:5000/api/match";
        [Export] public NodePath NetworkManagerPath;

        private NetworkManager _networkManager;

        private EMatchmakingState _currentState = EMatchmakingState.Idle;
        public EMatchmakingState CurrentState
        {
            get => _currentState;
            private set
            {
                if (_currentState != value)
                {
                    _currentState = value;
                    EmitSignal(SignalName.MatchmakingStateChanged, (int)_currentState);
                }
            }
        }

        public Guid CurrentMatchId { get; private set; }


        [Signal] public delegate void MatchmakingStateChangedEventHandler(int newState);
        [Signal] public delegate void MatchFoundEventHandler(string matchId);
        [Signal] public delegate void MatchStartedEventHandler(string matchId);
        [Signal] public delegate void MatchSearchingEventHandler();
        [Signal] public delegate void MatchErrorEventHandler(string errorMessage);

        public override void _Ready()
        {
            _networkManager = GetNode<NetworkManager>(NetworkManagerPath);

            // Registra i callback per i messaggi WebSocket e le risposte HTTP
            _networkManager.OnSignalRMessageReceived += OnSignalRMessageReceived;
        }

        /// <summary>
        /// Avvia il flusso di matchmaking inviando i dati del giocatore (scelta eroe e deck) al backend.
        /// </summary>
        /// <param name="playerId">Identificativo del giocatore (GUID)</param>
        /// <param name="heroId">Identificativo dell'eroe scelto (GUID)</param>
        /// <param name="deckCardIds">Deck selezionato, come lista di GUID delle carte</param>
        public void StartMatchmaking(Guid playerId, Guid heroId, List<Guid> deckCardIds)
        {
            CurrentState = EMatchmakingState.Searching;

            // Costruisci il TO della richiesta di matchmaking
            var matchmakingRequest = new
            {
                PlayerId = playerId,
                HeroId = heroId,
                DeckCardIds = deckCardIds
            };

            string jsonRequest = JsonSerializer.Serialize(matchmakingRequest);
            bool requestSent = _networkManager.SendHttpPostRequest(
                $"{RestApiUrl}/matchmaking",
                jsonRequest,
                OnMatchmakingResponseReceived
            );

            if (!requestSent)
            {
                EmitSignal(SignalName.MatchError, "Errore durante l'invio della richiesta di matchmaking");
                CurrentState = EMatchmakingState.Idle;
            }
            else
            {
                GD.Print("Richiesta di matchmaking inviata. In attesa di risposta...");
            }
        }

        /// <summary>
        /// Crea un nuovo match (host).
        /// </summary>
        /// <param name="playerId">Identificativo del giocatore (GUID)</param>
        /// <param name="heroId">Identificativo dell'eroe scelto (GUID)</param>
        /// <param name="deckCardIds">Deck selezionato, come lista di GUID delle carte</param>
        /// <param name="matchName">Nome del match (opzionale)</param>
        public void CreateMatch(Guid playerId, Guid heroId, List<Guid> deckCardIds, string matchName = "")
        {
            // Aggiorna lo stato
            CurrentState = EMatchmakingState.Searching;

            // Costruisci il DTO della richiesta di creazione match
            var createMatchRequest = new
            {
                PlayerId = playerId,
                HeroId = heroId,
                DeckCardIds = deckCardIds,
                MatchName = matchName
            };

            // Serializza il DTO in JSON
            string jsonRequest = JsonSerializer.Serialize(createMatchRequest);

            // Invia la richiesta REST con callback personalizzato
            bool requestSent = _networkManager.SendHttpPostRequest(
                $"{RestApiUrl}/create",
                jsonRequest,
                OnCreateMatchResponseReceived
            );

            if (!requestSent)
            {
                EmitSignal(SignalName.MatchError, "Errore durante l'invio della richiesta di creazione match");
                CurrentState = EMatchmakingState.Idle;
            }
            else
            {
                GD.Print("Richiesta di creazione match inviata. In attesa di risposta...");
            }
        }

        /// <summary>
        /// Partecipa a un match specifico (join).
        /// </summary>
        /// <param name="playerId">Identificativo del giocatore (GUID)</param>
        /// <param name="matchId">Identificativo del match (GUID)</param>
        /// <param name="heroId">Identificativo dell'eroe scelto (GUID)</param>
        /// <param name="deckCardIds">Deck selezionato, come lista di GUID delle carte</param>
        public void JoinMatch(Guid playerId, Guid matchId, Guid heroId, List<Guid> deckCardIds)
        {
            // Aggiorna lo stato
            CurrentState = EMatchmakingState.Searching;

            // Costruisci il DTO della richiesta di join match
            var joinMatchRequest = new
            {
                PlayerId = playerId,
                MatchId = matchId,
                HeroId = heroId,
                DeckCardIds = deckCardIds
            };

            // Serializza il DTO in JSON
            string jsonRequest = JsonSerializer.Serialize(joinMatchRequest);

            // Invia la richiesta REST con callback personalizzato
            bool requestSent = _networkManager.SendHttpPostRequest(
                $"{RestApiUrl}/join",
                jsonRequest,
                OnJoinMatchResponseReceived
            );

            if (!requestSent)
            {
                EmitSignal(SignalName.MatchError, "Errore durante l'invio della richiesta di join match");
                CurrentState = EMatchmakingState.Idle;
            }
            else
            {
                GD.Print("Richiesta di join match inviata. In attesa di risposta...");
            }
        }
       
        #region Callback Responses

        /// <summary>
        /// Gestisce la risposta della richiesta di matchmaking.
        /// </summary>
        private void OnMatchmakingResponseReceived(long result, long responseCode, string[] headers, byte[] body)
        {
            GD.Print("Risposta avvio matchmaking ricevuta.");
            if (responseCode == 200)
            {
                string responseText = Encoding.UTF8.GetString(body);
                try
                {
                    var responseData = JsonSerializer.Deserialize<Dictionary<string, string>>(responseText);
                    if (responseData != null && responseData.ContainsKey("matchId"))
                    {
                        CurrentMatchId = Guid.Parse(responseData["matchId"]);
                        GD.Print("Match creato. MatchId: " + CurrentMatchId);

                        // Notifica
                        EmitSignal(SignalName.MatchSearching);
                    }
                    else
                    {
                        EmitSignal(SignalName.MatchError, "Risposta creazione match non valida");
                        CurrentState = EMatchmakingState.Idle;
                    }
                }
                catch (Exception ex)
                {
                    EmitSignal(SignalName.MatchError, "Errore nel parsing della risposta: " + ex.Message);
                    CurrentState = EMatchmakingState.Idle;
                }
            }
            else
            {
                EmitSignal(SignalName.MatchError, "Creazione match fallita. Codice risposta: " + responseCode);
                CurrentState = EMatchmakingState.Idle;
            }
        }

        /// <summary>
        /// Gestisce la risposta della richiesta di creazione match.
        /// </summary>
        private void OnCreateMatchResponseReceived(long result, long responseCode, string[] headers, byte[] body)
        {
            GD.Print("Risposta creazione match ricevuta.");
            if (responseCode == 200)
            {
                string responseText = Encoding.UTF8.GetString(body);
                try
                {
                    var responseData = JsonSerializer.Deserialize<Dictionary<string, string>>(responseText);
                    if (responseData != null && responseData.ContainsKey("matchId"))
                    {
                        CurrentMatchId = Guid.Parse(responseData["matchId"]);
                        GD.Print("Match creato. MatchId: " + CurrentMatchId);

                        EmitSignal(SignalName.MatchSearching, CurrentMatchId.ToString());
                    }
                    else
                    {
                        EmitSignal(SignalName.MatchError, "Risposta creazione match non valida");
                        CurrentState = EMatchmakingState.Idle;
                    }
                }
                catch (Exception ex)
                {
                    EmitSignal(SignalName.MatchError, "Errore nel parsing della risposta: " + ex.Message);
                    CurrentState = EMatchmakingState.Idle;
                }
            }
            else
            {
                EmitSignal(SignalName.MatchError, "Creazione match fallita. Codice risposta: " + responseCode);
                CurrentState = EMatchmakingState.Idle;
            }
        }

        /// <summary>
        /// Gestisce la risposta della richiesta di join match.
        /// </summary>
        private void OnJoinMatchResponseReceived(long result, long responseCode, string[] headers, byte[] body)
        {
            GD.Print("Risposta join match ricevuta.");
            if (responseCode == 200)
            {
                string responseText = Encoding.UTF8.GetString(body);
                try
                {
                    var responseData = JsonSerializer.Deserialize<Dictionary<string, string>>(responseText);
                    if (responseData != null && responseData.ContainsKey("matchId"))
                    {
                        CurrentMatchId = Guid.Parse(responseData["matchId"]);
                        GD.Print("Match joined. MatchId: " + CurrentMatchId);

                        EmitSignal(SignalName.MatchSearching, CurrentMatchId.ToString());
                    }
                    else
                    {
                        EmitSignal(SignalName.MatchError, "Risposta join match non valida");
                        CurrentState = EMatchmakingState.Idle;
                    }
                }
                catch (Exception ex)
                {
                    EmitSignal(SignalName.MatchError, "Errore nel parsing della risposta: " + ex.Message);
                    CurrentState = EMatchmakingState.Idle;
                }
            }
            else
            {
                EmitSignal(SignalName.MatchError, "Join match fallito. Codice risposta: " + responseCode);
                CurrentState = EMatchmakingState.Idle;
            }
        }

        #endregion

        #region WebSocket Message Handling

        /// <summary>
        /// Gestisce i messaggi ricevuti tramite WebSocket.
        /// </summary>
        private void OnSignalRMessageReceived(Dictionary<string, object> messageData)
        {
            if (!messageData.TryGetValue("type", out var raw)) 
                return;

            switch ((EHubMessageType) raw)
            {
                case EHubMessageType.MatchStarted: HandleMatchStartedMessage(messageData); break;
                //case "MatchCanceled": HandleMatchCanceledMessage(messageData); break;
            }
        }

        private void HandleMatchStartedMessage(Dictionary<string, object> messageData)
        {
            if (!messageData.TryGetValue("matchId", out object value))
                return;

            string matchId = value.ToString();
            GD.Print($"Match avviato: {matchId}");

            CurrentState = EMatchmakingState.InMatch;
            EmitSignal(SignalName.MatchStarted, matchId);
        }

        #endregion
    }
}
