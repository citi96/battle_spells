namespace Battle_Spells.Models.Enums.Card
{
    public enum ECardType : ushort
    {
        Hero = 0,
        Spell = 10,
        Shop = 20,
        Minion = 30,
        Unknown = ushort.MaxValue,
    }
}
