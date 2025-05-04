using System;
using System.Buffers;
using System.Collections.Generic;
using BattleSpells.Scripts.Resources;
using Godot;

namespace BattleSpells.Scripts.Holders
{
    [GlobalClass]
    public partial class PlayerHolder : Resource
    {
        [Export] private NodePath HandGridPath { get; set; } = null!;

        private readonly List<string> _startingCards = [];
        private readonly List<CardDefinitionBase> _cardsOnHand = [];
        
        public Node HandGrid { get; private set; } = null!;

        public Guid Id { get; private set; }

        public void Init(Node owner)
        {
            HandGrid = owner.GetNode(HandGridPath);
        }

        public void AddCardToHand(CardDefinitionBase card)
        {
            _cardsOnHand.Add(card);
        }
    }
}
