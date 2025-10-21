namespace AdventOfCode.Year2015.Day22;

internal class SpellInfo
{
    public int Cost { get; set; }
    public int Damage { get; set; }
    public int Armor { get; set; }
    public int Heal { get; set; }
    public int ManaCharge { get; set; }
    public int EffectTurns { get; set; }

    public bool IsInstant => EffectTurns == 0;

    public SpellInfo(int cost, int damage, int armor, int heal, int manaCharge, int effectTurns)
    {
        Cost = cost;
        Damage = damage;
        Armor = armor;
        Heal = heal;
        ManaCharge = manaCharge;
        EffectTurns = effectTurns;
    }

    public void ApplyEffects(Player player, Boss boss)
    {
        boss.HitPoints -= Damage;
        player.HitPoints += Heal;
        player.Mana += ManaCharge;
    }
}
