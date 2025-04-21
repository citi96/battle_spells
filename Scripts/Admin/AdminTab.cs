using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Battle_Spells.model.DTOs;
using BattleSpells.Scripts.Helper;
using BattleSpells.Scripts.Managers;
using BattleSpells.Scripts.Resources;
using Godot;

namespace BattleSpells.Scripts.Admin
{
    public partial class AdminTab : Control
    {
        [Export] public string HeroesResourceFolder = "res://Resources/Heroes";
        [Export] public string CardsResourceFolder = "res://Resources/Cards";
        [Export] public NodePath SyncButtonPath;
        [Export] public string SyncEndpointUrl = "http://localhost:5000/api/admin/syncresources";

        private NetworkManager _networkManager;
        private Button _syncButton;

        public override void _Ready()
        {
            _syncButton = GetNode<Button>(SyncButtonPath);
            _syncButton.Pressed += OnSyncButtonPressed;

            _networkManager = GetNode<NetworkManager>("/root/NetworkManager");
        }

        /// <summary>
        /// Metodo chiamato da un pulsante "Sync Resources from Disk"
        /// </summary>
        public void OnSyncButtonPressed()
        {
            List<HeroDefinition> heroes = ResourceLoaderHelper.LoadResourcesFromFolder<HeroDefinition>(HeroesResourceFolder);
            List<CardDefinition> cards = ResourceLoaderHelper.LoadResourcesFromFolder<CardDefinition>(CardsResourceFolder);

            // Converti le risorse in formato DTO per il payload (esempio base)
            var heroResources = new List<HeroRequest>();
            foreach (var hero in heroes)
            {
                heroResources.Add(new(Guid.Empty, hero.HeroName, hero.BaseHP, hero.BaseOrbs, hero.Description));
            }

            var cardResources = new List<CardRequest>();
            foreach (var card in cards)
            {
                cardResources.Add(new(Guid.Empty, card.Name, card.Description, card.Cost, card.EffectDescription, card.Effects, card.Rarity, card.Type, card.HeroDefinition.Id));
            }

            var syncPayload = new SyncResourcesRequest(heroResources, cardResources);

            // Serializza il payload in JSON
            string jsonData = JsonSerializer.Serialize(syncPayload);
            GD.Print("Inviando risorse: " + jsonData);

            _networkManager.SendHttpPostRequest(SyncEndpointUrl, jsonData);
        }

        private void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
        {
            string responseText = Encoding.UTF8.GetString(body);
            if (responseCode == 200)
                GD.Print("Sincronizzazione avvenuta con successo: " + responseText);
            else
                GD.PrintErr("Errore di sincronizzazione. Codice: " + responseCode + " Risposta: " + responseText);
        }
    }
}
