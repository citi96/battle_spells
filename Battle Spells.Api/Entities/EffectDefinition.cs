using Battle_Spells.Models.Enums.Card;

namespace Battle_Spells.Api.Entities
{
    public class EffectDefinition : BaseEntity
    {
        public ECardEffectType EffectType { get; set; } = ECardEffectType.Unknown;
        public int Amount { get; set; }
        public string? SerializedParameters { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid? ParentEffectId { get; set; }
        public virtual ICollection<EffectDefinition>? SubEffects { get; set; }
        public virtual EffectDefinition? ConditionalEffect { get; set; }
        public Guid? ConditionalEffectId { get; set; }
        public ECardEffectCondition Condition { get; set; } = ECardEffectCondition.Unknown;
        public Guid? CardId { get; set; }
    }
}
