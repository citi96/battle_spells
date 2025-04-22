namespace Battle_Spells.Models.Enums.Card
{
    public enum ECardRarity : ushort
    {
        Common = 0,
        Uncommon = 10,
        Rare = 20,
        Epic = 30,
        Legendary = 40,
        Unknown = ushort.MaxValue,
    }
}
