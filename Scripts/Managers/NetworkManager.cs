using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Godot;
using HttpClient = Godot.HttpClient;

namespace BattleSpells.Scripts.Managers
{
    public partial class NetworkManager : Node
    {
        // Definizione dei delegati per i callback
        public delegate void WebSocketMessageReceived(Dictionary<string, object> messageData);
        public delegate void HttpResponseReceived(long result, long responseCode, string[] headers, byte[] body);

        // Eventi che altri oggetti possono sottoscrivere
        public event WebSocketMessageReceived OnWebSocketMessageReceived;
        public event HttpResponseReceived OnHttpResponseReceived;

        private readonly WebSocketMultiplayerPeer _peer = new();
        private bool _wsConnected = false;

        private HttpRequest _httpRequest;

        [Export] public string WebSocketUrl { get; set; } = "ws://localhost:8080";

        [Signal] public delegate void WebSocketConnectedEventHandler();
        [Signal] public delegate void WebSocketDisconnectedEventHandler();

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
            // Connetti il segnale di default per richieste HTTP
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
            EmitSignal(SignalName.WebSocketConnected);
        }

        private void OnPeerDisconnected(long id)
        {
            _wsConnected = false;
            GD.Print($"Peer disconnesso: {id}");
            EmitSignal(SignalName.WebSocketDisconnected);
        }

        private void OnConnectedToServer()
        {
            _wsConnected = true;
            GD.Print("Connesso al server WebSocket.");
            EmitSignal(SignalName.WebSocketConnected);
        }

        private void OnConnectionFailed()
        {
            _wsConnected = false;
            GD.PrintErr("Connessione al server WebSocket fallita.");
        }

        /// <summary>
        /// Invia un messaggio via WebSocket
        /// </summary>
        /// <param name="message">Messaggio da inviare in formato dizionario</param>
        /// <returns>True se il messaggio è stato inviato, false altrimenti</returns>
        public bool SendWebSocketMessage(Dictionary<string, object> message)
        {
            if (!_wsConnected)
            {
                GD.PrintErr("WebSocket non connesso; impossibile inviare il messaggio.");
                return false;
            }
            string jsonMsg = JsonSerializer.Serialize(message);
            byte[] data = Encoding.UTF8.GetBytes(jsonMsg);
            _peer.PutPacket(data);
            return true;
        }

        /// <summary>
        /// Elabora il messaggio ricevuto dal WebSocket
        /// </summary>
        /// <param name="message">Messaggio ricevuto in formato JSON</param>
        private void ProcessWebSocketMessage(string message)
        {
            try
            {
                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(message);
                if (data != null)
                {
                    // Notifica gli ascoltatori
                    OnWebSocketMessageReceived?.Invoke(data);

                    // Logica interna
                    if (data.TryGetValue("state", out object value))
                    {
                        GD.Print("Stato aggiornato: " + value.ToString());
                    }

                    // Puoi aggiungere logica specifica qui se necessario
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
        /// Invia una richiesta HTTP POST standard.
        /// </summary>
        /// <param name="url">URL della richiesta</param>
        /// <param name="jsonBody">Corpo della richiesta in formato JSON</param>
        /// <returns>True se la richiesta è stata inviata, false altrimenti</returns>
        public bool SendHttpPostRequest(string url, string jsonBody)
        {
            return SendHttpPostRequest(url, jsonBody, null);
        }

        /// <summary>
        /// Invia una richiesta HTTP POST con callback personalizzato.
        /// </summary>
        /// <param name="url">URL della richiesta</param>
        /// <param name="jsonBody">Corpo della richiesta in formato JSON</param>
        /// <param name="callback">Callback personalizzato da invocare alla risposta</param>
        /// <returns>True se la richiesta è stata inviata, false altrimenti</returns>
        public bool SendHttpPostRequest(string url, string jsonBody, HttpResponseReceived callback)
        {
            GD.Print("Invio richiesta HTTP POST a: " + url);

            // Se abbiamo un callback personalizzato, lo registriamo temporaneamente
            if (callback != null)
            {
                HttpRequest tempRequest = new HttpRequest();
                AddChild(tempRequest);
                tempRequest.RequestCompleted += (result, responseCode, headers, body) =>
                {
                    callback(result, responseCode, headers, body);
                    tempRequest.QueueFree(); // Pulizia
                };

                Error err = tempRequest.Request(
                    url,
                    ["Content-Type: application/json"],
                    HttpClient.Method.Post,
                    jsonBody
                );

                if (err != Error.Ok)
                {
                    GD.PrintErr("Errore nell'invio della richiesta HTTP: " + err);
                    tempRequest.QueueFree();
                    return false;
                }
                return true;
            }
            else
            {
                // Usa la richiesta HTTP standard
                Error err = _httpRequest.Request(
                    url,
                    ["Content-Type: application/json"],
                    HttpClient.Method.Post,
                    jsonBody
                );

                if (err != Error.Ok)
                {
                    GD.PrintErr("Errore nell'invio della richiesta HTTP: " + err);
                    return false;
                }
                return true;
            }
        }

        private void OnHttpRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
        {
            string responseText = Encoding.UTF8.GetString(body);
            GD.Print($"HTTP Response - Code: {responseCode} | Response: {responseText}");

            // Notifica gli ascoltatori
            OnHttpResponseReceived?.Invoke(result, responseCode, headers, body);
        }

        /// <summary>
        /// Invia una richiesta HTTP GET.
        /// </summary>
        public bool SendHttpGetRequest(string url)
        {
            GD.Print("Invio richiesta HTTP GET a: " + url);
            Error err = _httpRequest.Request(
                url,
                ["Content-Type: application/json"],
                HttpClient.Method.Get
            );

            if (err != Error.Ok)
            {
                GD.PrintErr("Errore nell'invio della richiesta HTTP GET: " + err);
                return false;
            }
            return true;
        }
        #endregion
    }
}
