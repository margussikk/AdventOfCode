using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2024.Day21;

[Puzzle(2024, 21, "Keypad Conundrum")]
public class Day21PuzzleSolver : IPuzzleSolver
{
    private string[] _doorCodes = [];

    public void ParseInput(string[] inputLines)
    {
        _doorCodes = inputLines;
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var numericKeypadRobot = new NumericKeypadRobot();

        var answer = 0;

        var numericKeypadCache = new Dictionary<(char?, char?), List<List<GridDirection>>>();
        var directionalKeypadCache = new Dictionary<(GridDirection?, GridDirection?), List<List<GridDirection>>>();

        foreach (var doorCode in _doorCodes)
        {
            var directionsList = numericKeypadRobot.GetDirections(numericKeypadCache, doorCode);

            for (var robot = 0; robot < 2; robot++)
            {
                var newDirectionsList = new List<List<GridDirection>>();

                foreach (var directions in directionsList)
                {
                    var directionalKeypadRobot = new DirectionalKeypadRobot();
                    var newDirections = directionalKeypadRobot.GetDirections(directionalKeypadCache, directions);

                    newDirectionsList.AddRange(newDirections);
                }

                directionsList.Clear();

                var shortest = newDirectionsList.Min(x => x.Count);
                foreach (var direction in newDirectionsList.Where(x => x.Count == shortest))
                {
                    directionsList.Add(direction);
                }
            }

            var complexity = directionsList[0].Count * int.Parse(doorCode[..^1]);
            answer += complexity;
        }

        return new PuzzleAnswer(answer, 242484);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        return new PuzzleAnswer("TODO", "TODO");
    }
}