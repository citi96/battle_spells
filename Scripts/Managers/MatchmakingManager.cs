using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;
using HttpClient = Godot.HttpClient;

namespace BattleSpells.Scripts.Managers
{
    /// <summary>
    /// Questo nodo gestisce il flusso di matchmaking: Find Match, Hero Selection, Deck Selection, Start match.
    /// </summary>
    public partial class MatchmakingManager : Node
    {
        [Export] public string RestApiUrl = "http://localhost:5000/api/match/matchmaking";
        [Export] public NodePath NetworkManagerPath;

        private HttpRequest _httpRequest;
        private NetworkManager _networkManager;

        public override void _Ready()
        {
            _httpRequest = GetNode<HttpRequest>("HTTPRequest");
            _networkManager = GetNode<BattleSpells.Scripts.Managers.NetworkManager>(NetworkManagerPath);

            // Collega il segnale per la risposta HTTP
            _httpRequest.RequestCompleted += OnRequestCompleted;
        }

        /// <summary>
        /// Avvia il flusso di matchmaking inviando i dati del giocatore (scelta eroe e deck) al backend.
        /// </summary>
        /// <param name="playerId">Identificativo del giocatore (GUID)</param>
        /// <param name="heroId">Identificativo dell’eroe scelto (GUID)</param>
        /// <param name="deckCardIds">Deck selezionato, come lista di GUID delle carte</param>
        public void StartMatchmaking(Guid playerId, Guid heroId, List<Guid> deckCardIds)
        {
            // Costruisci il DTO della richiesta di matchmaking
            var matchmakingRequest = new
            {
                PlayerId = playerId,
                HeroId = heroId,
                DeckCardIds = deckCardIds
            };

            // Serializza il DTO in JSON
            string jsonRequest = JsonSerializer.Serialize(matchmakingRequest);

            // Invia la richiesta REST
            Error err = _httpRequest.Request(
                RestApiUrl,
                new string[] { "Content-Type: application/json" },
                HttpClient.Method.Post,
                jsonRequest
            );

            if (err != Error.Ok)
            {
                GD.PrintErr("Errore durante l'invio della richiesta di matchmaking: " + err);
            }
            else
            {
                GD.Print("Richiesta di matchmaking inviata.");
            }
        }

        /// <summary>
        /// Gestisce la risposta della richiesta di matchmaking.
        /// Attende la risposta HTTP e, se tutto va bene, estrae il MatchId dal JSON e chiede al NetworkManager di unirsi al match.
        /// </summary>
        private void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
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
                        Guid matchId = Guid.Parse(responseData["matchId"]);
                        GD.Print("MatchId ottenuto: " + matchId);

                        // Usa il NetworkManager per inviare un messaggio di join al gruppo del match
                        var joinMsg = new Dictionary<string, object>
                        {
                            { "command", "JoinMatchGroup" },
                            { "matchId", matchId.ToString() }
                        };

                        _networkManager.SendWebSocketMessage(joinMsg);

                        // Qui puoi invocare logica per cambiare schermata o aggiornare l'interfaccia utente
                        GD.Print("Unito al gruppo match. Avvia il gameplay...");
                    }
                    else
                    {
                        GD.PrintErr("Risposta matchmaking non valida.");
                    }
                }
                catch (Exception ex)
                {
                    GD.PrintErr("Errore nel parsing della risposta: " + ex.Message);
                }
            }
            else
            {
                GD.PrintErr("Matchmaking fallito. Codice risposta: " + responseCode);
            }
        }
    }
}
