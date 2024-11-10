using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2023.Day02;

[Puzzle(2023, 2, "Cube Conundrum")]
public class Day02PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Game> _games = [];

    public void ParseInput(string[] inputLines)
    {
        _games = inputLines.Select(Game.Parse).ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var maximumCubeSet = new CubeSet
        {
            Red = 12,
            Green = 13,
            Blue = 14
        };

        var answer = _games
            .Where(game => game.CubeSets
                .All(cubeSet =>
                    cubeSet.Red <= maximumCubeSet.Red &&
                    cubeSet.Green <= maximumCubeSet.Green &&
                    cubeSet.Blue <= maximumCubeSet.Blue))
            .Select(x => x.Id)
            .Sum();

        return new PuzzleAnswer(answer, 2632);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _games
            .Select(g => g.Power)
            .Sum();

        return new PuzzleAnswer(answer, 69629);
    }
}
