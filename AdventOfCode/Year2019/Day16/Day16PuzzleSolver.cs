using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2019.Day16;

[Puzzle(2019, 16, "Flawed Frequency Transmission")]
public class Day16PuzzleSolver : IPuzzleSolver
{
    private readonly int[] _baseNumbers = [ 0, 1, 0, -1 ];

    private List<int> _numbers = [];

    public void ParseInput(string[] inputLines)
    {
        _numbers = inputLines[0].Select(character => character - '0').ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var numbers = new List<int>(_numbers);

        for (var phase = 1; phase <= 100; phase++)
        {
            numbers = ApplyFFT(numbers);
        }

        var first8Characters = numbers.Take(8)
                                      .Select(n => Convert.ToChar(n + '0'))
                                      .ToArray();

        var answer = new string(first8Characters);

        return new PuzzleAnswer(answer, "18933364");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var messageOffset = _numbers.Take(7)
                                    .Aggregate(0, (acc, curr) => acc * 10 + curr);

        var messageLength = _numbers.Count * 10000 - messageOffset;
        var partialLength = messageLength % _numbers.Count;
        var partialStart = _numbers.Count - partialLength;

        var numbers = new int[messageLength];

        _numbers[partialStart..].CopyTo(numbers);

        for (var index = partialLength; index < messageLength; index += _numbers.Count)
        {
            _numbers.CopyTo(numbers, index);
        }

        for (var phase = 1; phase <= 100; phase++)
        {
            numbers = ApplyFFT2(numbers);
        }


        var first8Characters = numbers.Take(8)
                                      .Select(n => Convert.ToChar(n + '0'))
                                      .ToArray();

        var answer = new string(first8Characters);

        return new PuzzleAnswer(answer, "28872305");
    }

    private List<int> ApplyFFT(List<int> numbers)
    {
        var newNumbers = new List<int>(numbers.Count);

        for (var repeat = 1; repeat <= numbers.Count; repeat++)
        {
            var output = numbers.Select((t, index) => t * _baseNumbers[(index + 1) / repeat % 4]).Sum();

            newNumbers.Add(Math.Abs(output) % 10);
        }

        return newNumbers;
    }

    private static int[] ApplyFFT2(int[] numbers)
    {
        var newNumbers = new int[numbers.Length];

        newNumbers[^1] = numbers[^1];

        for (var index = numbers.Length - 2; index >= 0; index--)
        {
            newNumbers[index] = (numbers[index] + newNumbers[index + 1]) % 10;
        }

        return newNumbers;
    }
}