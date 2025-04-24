namespace Battle_Spells.Models.Enums.Match
{
    public enum EMatchState : ushort
    {
        Created,
        Canceled,
        Started,
        Ended,
        Unknown = ushort.MaxValue,
    }
}
