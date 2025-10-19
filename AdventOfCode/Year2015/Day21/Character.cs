namespace AdventOfCode.Year2015.Day21;

internal class Character
{
    public int HitPoints { get; set; }
    public int Damage { get; set; }
    public int Armor { get; set; }

    public Character(int hitPoints, int damage, int armor)
    {
        HitPoints = hitPoints;
        Damage = damage;
        Armor = armor;
    }

    public Character Clone()
    {
        return new Character(HitPoints, Damage, Armor);
    }

    public static Character Parse(string[] inputLines)
    {
        var hitPoints = 0;
        var damage = 0;
        var armor = 0;

        foreach (var line in inputLines)
        {
            var parts = line.Split(": ");
            switch (parts[0])
            {
                case "Hit Points":
                    hitPoints = int.Parse(parts[1]);
                    break;
                case "Damage":
                    damage = int.Parse(parts[1]);
                    break;
                case "Armor":
                    armor = int.Parse(parts[1]);
                    break;
            }
        }

        return new Character(hitPoints, damage, armor);
    }
}
