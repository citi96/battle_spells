namespace Battle_Spells.Models.Enums.Match
{
    public enum EMatchState : ushort
    {
        Created,
        Canceled,
        Started,
        Unknown = ushort.MaxValue,
    }
}
