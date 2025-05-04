using System;
using BattleSpells.Scripts.Resources;
using Godot;
using Godot.Collections;

namespace BattleSpells.Scripts.Managers
{
    [GlobalClass]
    public partial class ResourcesManager : Resource
    {
        [Export] public Array<CardDefinitionBase> AllCards { get; private set; } = [];

        private readonly System.Collections.Generic.Dictionary<Guid, CardDefinitionBase> _cardDictionary = [];

        public void Init()
        {
            _cardDictionary.Clear();

            foreach (var card in AllCards)
            {
                card.InitCard();
                _cardDictionary[Guid.Parse(card.Id)] = card;
            }
        }
        public CardDefinitionBase? GetInstanceOfACard(Guid id)
        {
            var originalCard = GetOriginalCard(id);
            if(originalCard == null)
            {
                GD.PushError($"Card {id} not found in ResourcesManager.");
                return null;
            }

            return originalCard.Duplicate() as CardDefinitionBase;
        }

        private CardDefinitionBase? GetOriginalCard(Guid id)
        {
            if (_cardDictionary.TryGetValue(id, out var card))
            {
                return card;
            }

            GD.PushError($"Card {id} not found in ResourcesManager.");
            return null;
        }


    }
}
