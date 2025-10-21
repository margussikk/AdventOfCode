namespace AdventOfCode.Year2015.Day22;

internal class Player
{
    public int HitPoints { get; set; }
    public int Mana { get; set; }

    public Dictionary<SpellType, int> ActiveEffects { get; private set; } = [];
    public int SpentMana { get; set; }

    public Player Clone()
    {
        return new Player
        {
            HitPoints = HitPoints,
            Mana = Mana,
            ActiveEffects = new Dictionary<SpellType, int>(ActiveEffects),
            SpentMana = SpentMana
        };
    }
}
