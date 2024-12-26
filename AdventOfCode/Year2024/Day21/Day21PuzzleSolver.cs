using AdventOfCode.Framework.Puzzle;

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
        var answer = GetAnswer(2);

        return new PuzzleAnswer(answer, 242484);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(25);

        return new PuzzleAnswer(answer, 294209504640384L);
    }

    private long GetAnswer(int robots)
    {
        var answer = 0L;

        var numericKeypadSequencesCache = new Dictionary<(char, char), List<string>>();
        var directionalKeybadSequencesCache = new Dictionary<(char, char), List<string>>();

        var numericKeypadRobot = new KeypadRobot(numericKeypadSequencesCache, true);

        var currentRobot = numericKeypadRobot;

        for (var counter = 0; counter < robots; counter++)
        {
            var newRobot = new KeypadRobot(directionalKeybadSequencesCache, false);
            currentRobot.ControlledBy = newRobot;
            currentRobot = newRobot;
        }

        foreach (var doorCode in _doorCodes)
        {
            var length = numericKeypadRobot.CalculateLength(doorCode, 0);

            var complexity = length * int.Parse(doorCode[..^1]);
            answer += complexity;
        }

        return answer;
    }
}