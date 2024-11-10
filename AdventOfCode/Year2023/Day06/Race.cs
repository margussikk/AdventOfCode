namespace AdventOfCode.Year2023.Day06;

internal class Race
{
    public long Time { get; }
    public long Distance { get; }

    public Race(long time, long distance)
    {
        Time = time;
        Distance = distance;
    }

    public long CountWaysToBeatTheRecord()
    {
        // Find number of times when: time * (Time - time) > Distance

        // Quadratic formula
        // ax^2 + bx + c = 0
        // x1 = (-b + sqrt(b^2 -4ac)) / 2a
        // x2 = (-b - sqrt(b^2 -4ac)) / 2a

        // minTime and maxTime are times when distance record is not yet or not anymore beaten
        var minTime = Convert.ToInt64(Math.Floor((Time - Math.Sqrt(Time * Time - 4 * Distance)) / 2));
        var maxTime = Convert.ToInt64(Math.Ceiling((Time + Math.Sqrt(Time * Time - 4 * Distance)) / 2));

        return maxTime - minTime - 1;
    }
}
