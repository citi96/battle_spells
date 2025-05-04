using System;
using System.Collections.Generic;
using System.Linq;
using Battle_Spells.model.Enums.Hub;
using BattleSpells.Scripts.Managers;
using Godot;

namespace BattleSpells.Scripts.Helper
{
    /// <summary>
    /// Gestisce e delega i messaggi SignalR ai manager appropriati in base al tipo di messaggio.
    /// Implementa il pattern Singleton per un accesso globale.
    /// </summary>
    public partial class MessageDispatcher : Node
    {
        public static MessageDispatcher Instance { get; private set; } = null!;

        private readonly Dictionary<EHubMessageType, List<Action<Dictionary<string, object>>>> _messageHandlers = [];
        private readonly List<Func<Dictionary<string, object>, bool>> _genericHandlers = [];

        public override void _Ready()
        {
            if (Instance != null && Instance != this)
            {
                GD.PushError("[MessageDispatcher] Istanza multipla rilevata!");
                QueueFree();
                return;
            }

            Instance = this;
            GD.Print("[MessageDispatcher] Inizializzato");

            NetworkManager.Instance.OnSignalRMessageReceived += DispatchMessage;
        }

        public override void _ExitTree()
        {
            if (Instance == this)
            {
                // Pulisci i riferimenti quando il nodo viene distrutto
                if (NetworkManager.Instance != null)
                {
                    NetworkManager.Instance.OnSignalRMessageReceived -= DispatchMessage;
                }

                _messageHandlers.Clear();
                _genericHandlers.Clear();
                Instance = null!;
            }
        }

        /// <summary>
        /// Registra un handler per un tipo specifico di messaggio
        /// </summary>
        /// <param name="messageType">Il tipo di messaggio da gestire</param>
        /// <param name="handler">Funzione di callback che riceve il messaggio</param>
        public void RegisterHandler(EHubMessageType messageType, Action<Dictionary<string, object>> handler)
        {
            if (handler == null)
            {
                GD.PrintErr("[MessageDispatcher] Tentativo di registrare un handler nullo");
                return;
            }

            if (!_messageHandlers.TryGetValue(messageType, out var handlers))
            {
                handlers = [];
                _messageHandlers[messageType] = handlers;
            }

            if (!handlers.Contains(handler))
            {
                handlers.Add(handler);
                GD.Print($"[MessageDispatcher] Handler registrato per {messageType}");
            }   
            else
            {
                GD.PrintErr($"[MessageDispatcher] Handler già registrato per {messageType}");
            }
        }

        /// <summary>
        /// Registra un handler generico che può gestire qualsiasi tipo di messaggio
        /// </summary>
        /// <param name="handler">Funzione che restituisce true se il messaggio è stato gestito</param>
        public void RegisterGenericHandler(Func<Dictionary<string, object>, bool> handler)
        {
            if (handler == null)
            {
                GD.PrintErr("[MessageDispatcher] Tentativo di registrare un handler generico nullo");
                return;
            }

            if (!_genericHandlers.Contains(handler))
            {
                _genericHandlers.Add(handler);
                GD.Print("[MessageDispatcher] Handler generico registrato");
            }
        }

        /// <summary>
        /// Rimuove un handler per un tipo specifico di messaggio
        /// </summary>
        public void UnregisterHandler(EHubMessageType messageType, Action<Dictionary<string, object>> handler)
        {
            if (handler == null || !_messageHandlers.TryGetValue(messageType, out var handlers))
                return;

            if (handlers.Remove(handler))
            {
                GD.Print($"[MessageDispatcher] Handler rimosso per {messageType}");

                // Se non ci sono più handler, rimuovi la chiave
                if (handlers.Count == 0)
                {
                    _messageHandlers.Remove(messageType);
                }
            }
        }

        /// <summary>
        /// Rimuove un handler generico
        /// </summary>
        public void UnregisterGenericHandler(Func<Dictionary<string, object>, bool> handler)
        {
            if (handler == null)
                return;

            if (_genericHandlers.Remove(handler))
            {
                GD.Print("[MessageDispatcher] Handler generico rimosso");
            }
        }

        /// <summary>
        /// Dispatcha il messaggio agli handler registrati
        /// </summary>
        private void DispatchMessage(Dictionary<string, object> messageData)
        {
            var hasType = messageData.TryGetValue("type", out var typeRaw);

            // Prima prova a gestire con gli handler specifici per tipo
            if (hasType && typeRaw != null)
            {
                try
                {
                    var messageType = (EHubMessageType)typeRaw;

                    if (_messageHandlers.TryGetValue(messageType, out var handlers))
                    {
                        var handlersCopy = new List<Action<Dictionary<string, object>>>(handlers);

                        foreach (var handler in handlersCopy)
                        {
                            try
                            {
                                handler.Invoke(messageData);
                            }
                            catch (Exception ex)
                            {
                                GD.PrintErr($"[MessageDispatcher] Errore durante l'esecuzione dell'handler per {messageType}: {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        GD.Print($"[MessageDispatcher] Nessun handler registrato per il messaggio di tipo: {messageType}");
                    }
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"[MessageDispatcher] Errore nella conversione del tipo di messaggio: {ex.Message}");
                }
            }

            // Poi prova con gli handler generici
            foreach (var genericHandler in _genericHandlers)
            {
                try
                {
                    if (genericHandler(messageData))
                        return;
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"[MessageDispatcher] Errore durante l'esecuzione dell'handler generico: {ex.Message}");
                }
            }

            // Se arriviamo qui e non abbiamo un tipo, significa che nessuno ha gestito il messaggio
            if (!hasType)
            {
                GD.Print("[MessageDispatcher] Messaggio senza tipo non gestito");
            }
        }

        /// <summary>
        /// Registra tutti gli handler di un oggetto in una volta sola
        /// </summary>
        /// <param name="target">L'oggetto i cui metodi devono essere registrati</param>
        public void RegisterHandlers(object target)
        {
            if (target == null)
                return;

            var type = target.GetType();

            // Cerca tutti i metodi decorati con l'attributo MessageHandler
            foreach (var method in type.GetMethods())
            {
                var attribute = Attribute.GetCustomAttribute(method, typeof(MessageHandlerAttribute)) as MessageHandlerAttribute;
                if (attribute != null)
                {
                    // Controlla se il metodo ha la firma corretta
                    var parameters = method.GetParameters();
                    if (parameters.Length == 1 && parameters[0].ParameterType == typeof(Dictionary<string, object>))
                    {
                        var action = Delegate.CreateDelegate(
                            typeof(Action<Dictionary<string, object>>),
                            target,
                            method
                        ) as Action<Dictionary<string, object>>;

                        if (action != null)
                        {
                            RegisterHandler(attribute.MessageType, action);
                        }
                    }
                    else
                    {
                        GD.PrintErr($"[MessageDispatcher] Il metodo {method.Name} ha una firma non valida per MessageHandler");
                    }
                }
            }
        }

        /// <summary>
        /// Deregistra tutti gli handler di un oggetto in una volta sola
        /// </summary>
        public void UnregisterHandlers(object target)
        {
            if (target == null)
                return;

            // Trova e rimuovi tutti gli handler che puntano a metodi dell'oggetto target
            foreach (var kvp in _messageHandlers.ToArray())
            {
                var messageType = kvp.Key;
                var handlers = kvp.Value;

                foreach (var handler in handlers.ToArray())
                {
                    var del = handler as Delegate;
                    if (del != null && del.Target == target)
                    {
                        UnregisterHandler(messageType, handler);
                    }
                }
            }

            // Rimuovi anche gli handler generici
            foreach (var handler in _genericHandlers.ToArray())
            {
                var del = handler as Delegate;
                if (del != null && del.Target == target)
                {
                    UnregisterGenericHandler(handler);
                }
            }
        }
    }

    /// <summary>
    /// Attributo per marcare i metodi che gestiscono specifici tipi di messaggi
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MessageHandlerAttribute : Attribute
    {
        public EHubMessageType MessageType { get; }

        public MessageHandlerAttribute(EHubMessageType messageType)
        {
            MessageType = messageType;
        }
    }
}