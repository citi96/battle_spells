using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Godot;
using HttpClient = Godot.HttpClient;

namespace BattleSpells.Scripts.Managers
{
    // Questo manager funge da punto unico per la comunicazione di rete (WebSocket e HTTP).
    public partial class NetworkManager : Node
    {
        private readonly WebSocketMultiplayerPeer _peer = new();
        private bool _wsConnected = false;

        private HttpRequest _httpRequest;

        [Export] public string WebSocketUrl { get; set; } = "ws://localhost:8080";

        public override void _Ready()
        {
            // Inizializza la parte WebSocket:
            Multiplayer.MultiplayerPeer = _peer;
            Multiplayer.PeerConnected += OnPeerConnected;
            Multiplayer.PeerDisconnected += OnPeerDisconnected;
            Multiplayer.ConnectedToServer += OnConnectedToServer;
            Multiplayer.ConnectionFailed += OnConnectionFailed;

            // Avvia la connessione WebSocket:
            Error wsError = _peer.CreateClient(WebSocketUrl);
            if (wsError != Error.Ok)
            {
                GD.PrintErr($"Errore durante la connessione al server WebSocket: {wsError}");
            }

            // Inizializza la parte HTTP: crea un nodo HTTPRequest come child
            _httpRequest = new HttpRequest();
            AddChild(_httpRequest);
            // Puoi, se vuoi, connettere qui il segnale RequestCompleted oppure gestirlo internamente per ogni chiamata
            _httpRequest.RequestCompleted += OnHttpRequestCompleted;
        }

        public override void _Process(double delta)
        {
            // Aggiorna il peer WebSocket
            _peer.Poll();

            if (_wsConnected && _peer.GetAvailablePacketCount() > 0)
            {
                byte[] packet = _peer.GetPacket();
                string msg = Encoding.UTF8.GetString(packet);
                GD.Print("Messaggio ricevuto via WS: " + msg);
                ProcessWebSocketMessage(msg);
            }
        }

        #region WebSocket Methods

        private void OnPeerConnected(long id)
        {
            _wsConnected = true;
            GD.Print("Connesso al server tramite WebSocketMultiplayerPeer.");

            // Esempio: invia un messaggio per unirsi ad un gruppo
            var joinMsg = new Dictionary<string, object>
            {
                { "command", "JoinMatchGroup" },
                { "matchId", "match_example_id" }
            };
            SendWebSocketMessage(joinMsg);
        }

        private void OnPeerDisconnected(long id)
        {
            GD.Print($"Peer disconnesso: {id}");
        }

        private void OnConnectedToServer()
        {
            GD.Print("Connesso al server WebSocket.");
        }

        private void OnConnectionFailed()
        {
            _wsConnected = false;
            GD.PrintErr("Connessione al server WebSocket fallita.");
        }

        // Invia un messaggio via WebSocket
        public void SendWebSocketMessage(Dictionary<string, object> message)
        {
            if (!_wsConnected)
            {
                GD.PrintErr("WebSocket non connesso; impossibile inviare il messaggio.");
                return;
            }
            string jsonMsg = JsonSerializer.Serialize(message);
            byte[] data = Encoding.UTF8.GetBytes(jsonMsg);
            _peer.PutPacket(data);
        }

        // Elabora il messaggio ricevuto dal WebSocket
        private void ProcessWebSocketMessage(string message)
        {
            try
            {
                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(message);
                if (data != null && data.ContainsKey("state"))
                {
                    GD.Print("Stato aggiornato: " + data["state"].ToString());
                    // Qui puoi invocare metodi o emettere segnali per aggiornare l'interfaccia del gioco
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr("Errore nel parsing del messaggio WS: " + ex.Message);
            }
        }
        #endregion

        #region HTTP Methods

        /// <summary>
        /// Invia una richiesta HTTP POST.
        /// Il callback OnHttpRequestCompleted gestisce la risposta, ma puoi anche fornire un callback personalizzato se necessario.
        /// </summary>
        public void SendHttpPostRequest(string url, string jsonBody)
        {
            GD.Print("Invio richiesta HTTP POST a: " + url);
            Error err = _httpRequest.Request(
                url,
                ["Content-Type: application/json"],
                HttpClient.Method.Post,
                jsonBody
            );
            if (err != Error.Ok)
            {
                GD.PrintErr("Errore nell'invio della richiesta HTTP: " + err);
            }
        }
        private void OnHttpRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
        {
            string responseText = Encoding.UTF8.GetString(body);
            GD.Print($"HTTP Response - Code: {responseCode} | Response: {responseText}");
            // Qui puoi decidere di invocare un delegato o un segnale per restituire la risposta al chiamante
        }

        // Puoi aggiungere altri metodi per GET, PUT, DELETE, ecc., se necessario.
        #endregion

        #region Utility & Delegation
        // Potresti aggiungere eventi e metodi per notificare altri script quando arriva una risposta HTTP o un messaggio WS.
        #endregion
    }
}
