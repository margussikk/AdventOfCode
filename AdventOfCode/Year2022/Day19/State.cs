namespace AdventOfCode.Year2022.Day19;

internal class State
{
    public int MinutesLeft { get; set; }

    public int Ore { get; set; }

    public int Clay { get; set; }

    public int Obsidian { get; set; }

    public int Geode { get; set; }

    public int OreRobots { get; set; }

    public int ClayRobots { get; set; }

    public int ObsidianRobots { get; set; }

    public int GeodeRobots { get; set; }

    public State? PreviousState { get; set; }

    public IEnumerable<State> GetNextStates(Blueprint blueprint)
    {
        var states = new List<State>();

        State state;

        // Try to build ore robot
        state = GetNextState(blueprint, RobotType.OreRobot);
        states.Add(state);

        // Try to build clay robot
        state = GetNextState(blueprint, RobotType.ClayRobot);
        states.Add(state);

        // Try to build obsidian robot
        state = GetNextState(blueprint, RobotType.ObsidianRobot);
        states.Add(state);

        // Try to build geode robot
        state = GetNextState(blueprint, RobotType.GeodeRobot);
        states.Add(state);

        return states;
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        hashCode.Add(MinutesLeft);
        hashCode.Add(Ore);
        hashCode.Add(Clay);
        hashCode.Add(Obsidian);
        hashCode.Add(Geode);
        hashCode.Add(OreRobots);
        hashCode.Add(ClayRobots);
        hashCode.Add(ObsidianRobots);
        hashCode.Add(GeodeRobots);

        return hashCode.ToHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is State otherState)
        {
            return MinutesLeft == otherState.MinutesLeft &&
                Ore == otherState.Ore &&
                Clay == otherState.Clay &&
                Obsidian == otherState.Obsidian &&
                Geode == otherState.Geode &&
                OreRobots == otherState.OreRobots &&
                ClayRobots == otherState.ClayRobots &&
                ObsidianRobots == otherState.ObsidianRobots &&
                GeodeRobots == otherState.GeodeRobots;
        }

        return false;
    }

    public void Print()
    {
        Console.WriteLine($"MinutesLeft: {MinutesLeft}");

        Console.WriteLine($"Ore: {Ore}");
        Console.WriteLine($"Clay: {Clay}");
        Console.WriteLine($"Obsidian: {Obsidian}");
        Console.WriteLine($"Geode: {Geode}");

        Console.WriteLine($"Ore robots: {OreRobots}");
        Console.WriteLine($"Clay robots: {ClayRobots}");
        Console.WriteLine($"Obsidian robots: {ObsidianRobots}");
        Console.WriteLine($"Geode robots: {GeodeRobots}");

        Console.WriteLine();
    }

    private State GetNextState(Blueprint blueprint, RobotType robotType)
    {
        var minutesSpent = MinutesLeft; // Fast forward to end if we can't build anything

        var oreCost = 0;
        var clayCost = 0;
        var obsidianCost = 0;

        var builtOreRobots = 0;
        var builtClayRobots = 0;
        var builtObsidianRobots = 0;
        var builtGeodeRobots = 0;


        if (robotType == RobotType.OreRobot && OreRobots < blueprint.MaxOreNeededPerMinute)
        {
            // Ore robot
            var minutesUntilEnoughOre = CalculateMinutesUntilEnoughMaterials(Ore, blueprint.OreRobotCostOre, OreRobots);

            var currentMinutesSpent = minutesUntilEnoughOre + 1;
            if (MinutesLeft - currentMinutesSpent > 0)
            {
                minutesSpent = currentMinutesSpent;
                oreCost = blueprint.OreRobotCostOre;
                builtOreRobots = 1;
            }
        }
        else if (robotType == RobotType.ClayRobot && ClayRobots < blueprint.ObsidianRobotCostClay)
        {
            // Clay robot
            var minutesUntilEnoughOre = CalculateMinutesUntilEnoughMaterials(Ore, blueprint.ClayRobotCostOre, OreRobots);

            var currentMinutesSpent = minutesUntilEnoughOre + 1;
            if (MinutesLeft - currentMinutesSpent > 0)
            {
                minutesSpent = currentMinutesSpent;
                oreCost = blueprint.ClayRobotCostOre;
                builtClayRobots = 1;
            }
        }
        else if (robotType == RobotType.ObsidianRobot && ClayRobots > 0 && ObsidianRobots < blueprint.GeodeRobotCostObsidian)
        {
            // Obsidian robot
            var minutesUntilEnoughOre = CalculateMinutesUntilEnoughMaterials(Ore, blueprint.ObsidianRobotCostOre, OreRobots);
            var minutesUntilEnoughClay = CalculateMinutesUntilEnoughMaterials(Clay, blueprint.ObsidianRobotCostClay, ClayRobots);
            var minutesUntilEnoughMaterials = int.Max(minutesUntilEnoughOre, minutesUntilEnoughClay);

            var currentMinutesSpent = minutesUntilEnoughMaterials + 1;
            if (MinutesLeft - currentMinutesSpent > 0)
            {
                minutesSpent = currentMinutesSpent;
                oreCost = blueprint.ObsidianRobotCostOre;
                clayCost = blueprint.ObsidianRobotCostClay;
                builtObsidianRobots = 1;
            }
        }
        else if (robotType == RobotType.GeodeRobot && ObsidianRobots > 0)
        {
            // Geode robot
            var minutesUntilEnoughOre = CalculateMinutesUntilEnoughMaterials(Ore, blueprint.GeodeRobotCostOre, OreRobots);
            var minutesUntilEnoughObsidian = CalculateMinutesUntilEnoughMaterials(Obsidian, blueprint.GeodeRobotCostObsidian, ObsidianRobots);
            var minutesUntilEnoughMaterials = int.Max(minutesUntilEnoughOre, minutesUntilEnoughObsidian);

            var currentMinutesSpent = minutesUntilEnoughMaterials + 1;
            if (MinutesLeft - currentMinutesSpent > 0)
            {
                minutesSpent = currentMinutesSpent;
                oreCost = blueprint.GeodeRobotCostOre;
                obsidianCost = blueprint.GeodeRobotCostObsidian;
                builtGeodeRobots = 1;
            }
        }

        var state = new State
        {
            MinutesLeft = MinutesLeft - minutesSpent,

            Ore = Ore + (minutesSpent * OreRobots) - oreCost,
            Clay = Clay + (minutesSpent * ClayRobots) - clayCost,
            Obsidian = Obsidian + (minutesSpent * ObsidianRobots) - obsidianCost,
            Geode = Geode + (minutesSpent * GeodeRobots),

            OreRobots = OreRobots + builtOreRobots,
            ClayRobots = ClayRobots + builtClayRobots,
            ObsidianRobots = ObsidianRobots + builtObsidianRobots,
            GeodeRobots = GeodeRobots + builtGeodeRobots,

            PreviousState = this,
        };

        // Don't keep more resources than needed.
        // This should cause more cache hits and speed things up
        state.Ore = int.Min(state.Ore, blueprint.MaxOreNeededPerMinute);
        state.Clay = int.Min(state.Clay, (state.MinutesLeft * blueprint.ObsidianRobotCostClay) - (state.ClayRobots * (state.MinutesLeft - 1)));
        state.Obsidian = int.Min(state.Obsidian, (state.MinutesLeft * blueprint.GeodeRobotCostObsidian) - (state.ObsidianRobots * (state.MinutesLeft - 1)));

        return state;

        // local helper method
        static int CalculateMinutesUntilEnoughMaterials(int materialCollected, int materialsCost, int robots)
        {
            return int.Max(Convert.ToInt32(Math.Ceiling(Convert.ToDouble(materialsCost - materialCollected) / robots)), 0);
        }
    }
}

