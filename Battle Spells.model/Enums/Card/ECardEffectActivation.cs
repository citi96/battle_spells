namespace Battle_Spells.Models.Enums.Card
{
    public enum ECardEffectActivation : ushort
    {
        Lazy,
        DeathEcho,
        EndOfTurn,
        Unknown = ushort.MaxValue,
    }
}
