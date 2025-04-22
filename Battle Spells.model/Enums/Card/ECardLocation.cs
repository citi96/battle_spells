namespace Battle_Spells.Models.Enums.Card
{
    public enum ECardLocation : ushort
    {
        Deck,
        Hand,
        Field,
        Graveyard,
        Unknown = ushort.MaxValue
    }
}
