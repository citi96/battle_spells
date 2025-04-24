using Battle_Spells.model.Enums.Hub;
using Godot;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HttpClient = Godot.HttpClient;

namespace BattleSpells.Scripts.Managers
{
    public partial class NetworkManager : Node
    {
        public delegate void SignalRMessageReceived(Dictionary<string, object> messageData);
        public delegate void HttpResponseReceived(long result, long responseCode, string[] headers, byte[] body);

        // Eventi che altri oggetti possono sottoscrivere
        public event SignalRMessageReceived OnSignalRMessageReceived;
        public event HttpResponseReceived OnHttpResponseReceived;

        [Export] public string RestBaseUrl { get; set; } = "http://localhost:5000";
        [Export] public string HubUrl { get; set; } = "ws://localhost:5000/ws";
        [Export] public string PlayerJwt { get; set; } = string.Empty;

        private HubConnection _hub;
        private HttpRequest _httpRequest;


        public override async void _Ready()
        {
            await InitSignalR();

            _httpRequest = new HttpRequest();
            AddChild(_httpRequest);
            _httpRequest.RequestCompleted += OnHttpRequestCompleted;
        }

        public override void _ExitTree()
        {
            _ = _hub?.DisposeAsync();
        }

        public override void _Process(double delta)
        {
            
        }

        #region SignalR Methods

        private async Task InitSignalR()
        {
            var builder = new HubConnectionBuilder()
                .WithUrl(HubUrl, options =>
                {
                    if (!string.IsNullOrWhiteSpace(PlayerJwt))
                    {
                        options.AccessTokenProvider = () => Task.FromResult(PlayerJwt);
                    }
                })
                .WithAutomaticReconnect();

            _hub = builder.Build();

            // Handler generico: tutti i push passano da qui
            _hub.On<object>(EHubEvent.ReceiveMatchEvent.ToString(), raw =>
            {
                try
                {
                    var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(raw.ToString());
                    if (dict != null)
                        OnSignalRMessageReceived?.Invoke(dict);
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"Errore parsing push SignalR: {ex.Message}");
                }
            });

            // Connessione
            try
            {
                await _hub.StartAsync();
                GD.Print($"[SignalR] Connesso – ConnectionId {_hub.ConnectionId}");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[SignalR] Connessione fallita: {ex.Message}");
            }
        }

        /// <summary>
        /// Non mi serve la tengo come template per le prossime chiamate
        /// </summary>
        public async Task JoinMatchGroup(Guid matchId)
        {
            if (_hub == null || _hub.State != HubConnectionState.Connected)
            {
                GD.PrintErr("[SignalR] Non connesso: impossibile JoinMatchGroup");
                return;
            }

            try
            {
                await _hub.InvokeAsync("JoinMatchGroup", matchId.ToString());
                GD.Print($"[SignalR] JoinMatchGroup {matchId}");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[SignalR] JoinMatchGroup errore: {ex.Message}");
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
