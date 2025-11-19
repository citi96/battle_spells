namespace Battle_Spells.Models.Enums.Card
{
    public enum ECardEffectActivation : ushort
    {
        Lazy,
        DeathEcho,
        EndOfTurn,
        OnPlay,
        OnDeath,
        OnAllyDeath,
        OnDamageTaken,
        Unknown = ushort.MaxValue,
    }
}
