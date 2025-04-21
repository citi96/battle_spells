using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Godot;

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
        public enum MatchmakingState
        {
            Idle,
            Searching,
            MatchFound,
            InMatch
        }

        private MatchmakingState _currentState = MatchmakingState.Idle;
        public MatchmakingState CurrentState
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
        [Signal] public delegate void MatchCreatedEventHandler(string matchId);
        [Signal] public delegate void MatchJoinedEventHandler(string matchId);
        [Signal] public delegate void MatchStartedEventHandler(string matchId);
        [Signal] public delegate void MatchErrorEventHandler(string errorMessage);

        public override void _Ready()
        {
            _networkManager = GetNode<NetworkManager>(NetworkManagerPath);

            // Registra i callback per i messaggi WebSocket e le risposte HTTP
            _networkManager.OnWebSocketMessageReceived += OnWebSocketMessageReceived;
        }

        /// <summary>
        /// Avvia il flusso di matchmaking inviando i dati del giocatore (scelta eroe e deck) al backend.
        /// </summary>
        /// <param name="playerId">Identificativo del giocatore (GUID)</param>
        /// <param name="heroId">Identificativo dell'eroe scelto (GUID)</param>
        /// <param name="deckCardIds">Deck selezionato, come lista di GUID delle carte</param>
        public void StartMatchmaking(Guid playerId, Guid heroId, List<Guid> deckCardIds)
        {
            // Aggiorna lo stato
            CurrentState = MatchmakingState.Searching;

            // Costruisci il DTO della richiesta di matchmaking
            var matchmakingRequest = new
            {
                PlayerId = playerId,
                HeroId = heroId,
                DeckCardIds = deckCardIds
            };

            // Serializza il DTO in JSON
            string jsonRequest = JsonSerializer.Serialize(matchmakingRequest);

            // Invia la richiesta REST con callback personalizzato
            bool requestSent = _networkManager.SendHttpPostRequest(
                $"{RestApiUrl}/matchmaking",
                jsonRequest,
                OnMatchmakingResponseReceived
            );

            if (!requestSent)
            {
                EmitSignal(SignalName.MatchError, "Errore durante l'invio della richiesta di matchmaking");
                CurrentState = MatchmakingState.Idle;
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
            CurrentState = MatchmakingState.Searching;

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
                CurrentState = MatchmakingState.Idle;
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
            CurrentState = MatchmakingState.Searching;

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
                CurrentState = MatchmakingState.Idle;
            }
            else
            {
                GD.Print("Richiesta di join match inviata. In attesa di risposta...");
            }
        }

        /// <summary>
        /// Avvia un match (richiesta solo dall'host).
        /// </summary>
        /// <param name="matchId">Identificativo del match (GUID)</param>
        public void StartMatch(Guid matchId)
        {
            // Costruisci il DTO della richiesta di start match
            var startMatchRequest = new
            {
                MatchId = matchId
            };

            // Serializza il DTO in JSON
            string jsonRequest = JsonSerializer.Serialize(startMatchRequest);

            // Invia la richiesta REST con callback personalizzato
            bool requestSent = _networkManager.SendHttpPostRequest(
                $"{RestApiUrl}/start",
                jsonRequest,
                OnStartMatchResponseReceived
            );

            if (!requestSent)
            {
                EmitSignal(SignalName.MatchError, "Errore durante l'invio della richiesta di avvio match");
            }
            else
            {
                GD.Print("Richiesta di avvio match inviata. In attesa di risposta...");
            }
        }

        /// <summary>
        /// Ottiene la lista dei match disponibili.
        /// </summary>
        public void GetAvailableMatches()
        {
            bool requestSent = _networkManager.SendHttpGetRequest($"{RestApiUrl}/available");

            if (!requestSent)
            {
                EmitSignal(SignalName.MatchError, "Errore durante l'ottenimento dei match disponibili");
            }
        }

        #region Callback Responses

        /// <summary>
        /// Gestisce la risposta della richiesta di matchmaking.
        /// </summary>
        private void OnMatchmakingResponseReceived(long result, long responseCode, string[] headers, byte[] body)
        {
            GD.Print("Risposta matchmaking ricevuta.");
            if (responseCode == 200)
            {
                string responseText = Encoding.UTF8.GetString(body);
                try
                {
                    // Assumiamo che il JSON di risposta contenga una proprietà "matchId" come stringa
                    var responseData = JsonSerializer.Deserialize<Dictionary<string, string>>(responseText);
                    if (responseData != null && responseData.ContainsKey("matchId"))
                    {
                        CurrentMatchId = Guid.Parse(responseData["matchId"]);
                        GD.Print("MatchId ottenuto: " + CurrentMatchId);

                        // Aggiorna lo stato
                        CurrentState = MatchmakingState.MatchFound;

                        // Notifica
                        EmitSignal(SignalName.MatchFound, CurrentMatchId.ToString());

                        // Usa il NetworkManager per inviare un messaggio di join al gruppo del match
                        JoinMatchGroup(CurrentMatchId);
                    }
                    else
                    {
                        EmitSignal(SignalName.MatchError, "Risposta matchmaking non valida");
                        CurrentState = MatchmakingState.Idle;
                    }
                }
                catch (Exception ex)
                {
                    EmitSignal(SignalName.MatchError, "Errore nel parsing della risposta: " + ex.Message);
                    CurrentState = MatchmakingState.Idle;
                }
            }
            else
            {
                EmitSignal(SignalName.MatchError, "Matchmaking fallito. Codice risposta: " + responseCode);
                CurrentState = MatchmakingState.Idle;
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

                        // Aggiorna lo stato
                        CurrentState = MatchmakingState.MatchFound;

                        // Notifica
                        EmitSignal(SignalName.MatchCreated, CurrentMatchId.ToString());

                        // Usa il NetworkManager per inviare un messaggio di join al gruppo del match
                        JoinMatchGroup(CurrentMatchId);
                    }
                    else
                    {
                        EmitSignal(SignalName.MatchError, "Risposta creazione match non valida");
                        CurrentState = MatchmakingState.Idle;
                    }
                }
                catch (Exception ex)
                {
                    EmitSignal(SignalName.MatchError, "Errore nel parsing della risposta: " + ex.Message);
                    CurrentState = MatchmakingState.Idle;
                }
            }
            else
            {
                EmitSignal(SignalName.MatchError, "Creazione match fallita. Codice risposta: " + responseCode);
                CurrentState = MatchmakingState.Idle;
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

                        // Aggiorna lo stato
                        CurrentState = MatchmakingState.MatchFound;

                        // Notifica
                        EmitSignal(SignalName.MatchJoined, CurrentMatchId.ToString());

                        // Usa il NetworkManager per inviare un messaggio di join al gruppo del match
                        JoinMatchGroup(CurrentMatchId);
                    }
                    else
                    {
                        EmitSignal(SignalName.MatchError, "Risposta join match non valida");
                        CurrentState = MatchmakingState.Idle;
                    }
                }
                catch (Exception ex)
                {
                    EmitSignal(SignalName.MatchError, "Errore nel parsing della risposta: " + ex.Message);
                    CurrentState = MatchmakingState.Idle;
                }
            }
            else
            {
                EmitSignal(SignalName.MatchError, "Join match fallito. Codice risposta: " + responseCode);
                CurrentState = MatchmakingState.Idle;
            }
        }

        /// <summary>
        /// Gestisce la risposta della richiesta di avvio match.
        /// </summary>
        private void OnStartMatchResponseReceived(long result, long responseCode, string[] headers, byte[] body)
        {
            GD.Print("Risposta avvio match ricevuta.");
            if (responseCode == 200)
            {
                GD.Print("Match avviato con successo. MatchId: " + CurrentMatchId);
                // La notifica di avvio match verrà generalmente ricevuta via WebSocket da tutti i client
            }
            else
            {
                EmitSignal(SignalName.MatchError, "Avvio match fallito. Codice risposta: " + responseCode);
            }
        }

        #endregion

        #region WebSocket Message Handling

        /// <summary>
        /// Gestisce i messaggi ricevuti tramite WebSocket.
        /// </summary>
        private void OnWebSocketMessageReceived(Dictionary<string, object> messageData)
        {
            // Verifica il tipo di messaggio
            if (messageData.ContainsKey("type"))
            {
                string messageType = messageData["type"].ToString();

                switch (messageType)
                {
                    case "MatchStarted":
                        HandleMatchStartedMessage(messageData);
                        break;
                    case "MatchState":
                        HandleMatchStateMessage(messageData);
                        break;
                    case "PlayerJoined":
                        HandlePlayerJoinedMessage(messageData);
                        break;
                    case "PlayerLeft":
                        HandlePlayerLeftMessage(messageData);
                        break;
                    default:
                        GD.Print($"Messaggio WebSocket sconosciuto: {messageType}");
                        break;
                }
            }
        }

        private void HandleMatchStartedMessage(Dictionary<string, object> messageData)
        {
            if (messageData.ContainsKey("matchId"))
            {
                string matchId = messageData["matchId"].ToString();
                GD.Print($"Match avviato: {matchId}");

                // Aggiorna lo stato
                CurrentState = MatchmakingState.InMatch;

                // Notifica
                EmitSignal(SignalName.MatchStarted, matchId);
            }
        }

        private void HandleMatchStateMessage(Dictionary<string, object> messageData)
        {
            // Gestisci gli aggiornamenti di stato del match
            // Qui potresti decodificare lo stato del match e aggiornare il gioco
            GD.Print("Ricevuto aggiornamento dello stato del match");
        }

        private void HandlePlayerJoinedMessage(Dictionary<string, object> messageData)
        {
            if (messageData.ContainsKey("playerId"))
            {
                string playerId = messageData["playerId"].ToString();
                GD.Print($"Player entrato nel match: {playerId}");
            }
        }

        private void HandlePlayerLeftMessage(Dictionary<string, object> messageData)
        {
            if (messageData.ContainsKey("playerId"))
            {
                string playerId = messageData["playerId"].ToString();
                GD.Print($"Player uscito dal match: {playerId}");
            }
        }

        #endregion

        /// <summary>
        /// Invia un messaggio per entrare nel gruppo WebSocket del match.
        /// </summary>
        private void JoinMatchGroup(Guid matchId)
        {
            // Usa il NetworkManager per inviare un messaggio di join al gruppo del match
            var joinMsg = new Dictionary<string, object>
            {
                { "command", "JoinMatchGroup" },
                { "matchId", matchId.ToString() }
            };

            bool sent = _networkManager.SendWebSocketMessage(joinMsg);
            if (sent)
            {
                GD.Print("Richiesta di join al gruppo match inviata.");
            }
            else
            {
                EmitSignal(SignalName.MatchError, "Errore nell'invio della richiesta di join al gruppo match");
            }
        }
    }
}
