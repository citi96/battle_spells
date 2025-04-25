namespace Battle_Spells.Models.Enums.Card
{
    public enum ECardEffectType : ushort
    {
        Damage,
        Heal,
        Leech,
        Draw,
        Discard,
        Death,
        Conditional,
        Shield,

        Unknown = ushort.MaxValue,
    }
}
