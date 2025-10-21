namespace AdventOfCode.Year2015.Day22;

internal class Boss
{
    public int HitPoints { get; set; }
    public int Damage { get; set; }

    public Boss Clone()
    {
        return new Boss
        {
            HitPoints = HitPoints,
            Damage = Damage
        };
    }

    public static Boss Parse(string[] inputLines)
    {
        var boss = new Boss();

        foreach (var line in inputLines)
        {
            var parts = line.Split(": ");
            switch (parts[0])
            {
                case "Hit Points":
                    boss.HitPoints = int.Parse(parts[1]);
                    break;
                case "Damage":
                    boss.Damage = int.Parse(parts[1]);
                    break;
            }
        }

        return boss;
    }
}
