using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2019.Day01;

[Puzzle(2019, 1, "The Tyranny of the Rocket Equation")]
public class Day01PuzzleSolver : IPuzzleSolver
{
    private List<int> _moduleMasses = [];

    public void ParseInput(string[] inputLines)
    {
        _moduleMasses = inputLines.Select(int.Parse)
                                  .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _moduleMasses.Sum(CalculateFuelRequirement);

        return new PuzzleAnswer(answer, 3456641);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _moduleMasses.Sum(CalculateTotalFuelRequirement);

        return new PuzzleAnswer(answer, 5182078);
    }

    private static int CalculateFuelRequirement(int mass)
    {
        return mass / 3 - 2;
    }

    private static int CalculateTotalFuelRequirement(int moduleMass)
    {
        var totalFuel = 0;
        var fuel = moduleMass;

        while(fuel > 0)
        {
            fuel = CalculateFuelRequirement(fuel);

            if (fuel > 0)
            {
                totalFuel += fuel;
            }
        }

        return totalFuel;
    }    
}