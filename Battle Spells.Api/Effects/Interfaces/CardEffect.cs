using Battle_Spells.Api.Effects.Context;
using Battle_Spells.Api.Entities;
using Battle_Spells.Models.Enums.Card;
using Battle_Spells.Models.Enums.Match;

namespace Battle_Spells.Api.Effects.Interfaces
{
    public abstract class CardEffect : ICardEffect
    {
        public abstract ECardEffectType EffectType { get; }

        public abstract Task<EffectResult> ExecuteAsync(EffectContext context);

        // Crea e traccia un cambiamento di stato
        protected MatchStateChange AddStateChange(EffectContext context, EMatchStateChangeType stateChangeType, Guid targetId, object data)
        {
            var change = new MatchStateChange
            {
                MatchId = context.Match.Id,
                ActionId = context.Action.Id,
                StateChangeType = stateChangeType,
                TargetId = targetId,
                SerializedData = System.Text.Json.JsonSerializer.Serialize(data),
                Sequence = context.SequenceCounter++
            };

            context.StateChanges.Add(change);
            return change;
        }
    }
}
