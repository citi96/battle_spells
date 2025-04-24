namespace Battle_Spells.model.Enums.Hub
{
    public enum EHubMessageType : ushort
    {
        MatchStarted,
        MatchEnded,
        MatchCanceled,
        Unknown = ushort.MaxValue
    }
}
