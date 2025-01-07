using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2023.Day18;

[Puzzle(2023, 18, "Lavaduct Lagoon")]
public class Day18PuzzleSolver : IPuzzleSolver
{
    private List<Instruction> _part1Instructions = [];
    private List<Instruction> _part2Instructions = [];

    public void ParseInput(string[] inputLines)
    {
        _part1Instructions = inputLines
            .Select(Instruction.Part1Parse)
            .ToList();

        _part2Instructions = inputLines
            .Select(Instruction.Part2Parse)
            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = CalculateLagoonCapacity(_part1Instructions);

        return new PuzzleAnswer(answer, 35991);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = CalculateLagoonCapacity(_part2Instructions);

        return new PuzzleAnswer(answer, 54058824661845L);
    }

    private static long CalculateLagoonCapacity(List<Instruction> instructions)
    {
        var coordinates = new List<GridCoordinate>();

        var startCoordinate = new GridCoordinate(0, 0);
        var lagoonWalker = new GridWalker(startCoordinate, startCoordinate, GridDirection.None, 0);
        foreach (var instruction in instructions)
        {
            lagoonWalker.Move(instruction.Direction, instruction.Distance);
            coordinates.Add(lagoonWalker.Coordinate);
        }

        var area = MeasurementFunctions.ShoelaceFormula(coordinates);

        var boundaryPoints = lagoonWalker.Steps;
        var interiorPoints = MeasurementFunctions.PicksTheoremGetInteriorPoints(area, lagoonWalker.Steps);

        return boundaryPoints + interiorPoints;
    }
}
