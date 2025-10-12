using AdventOfCode.Framework.Puzzle;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Year2015.Day04;

[Puzzle(2015, 4, "The Ideal Stocking Stuffer")]
public class Day04PuzzleSolver : IPuzzleSolver
{
    private string _input = string.Empty;

    public void ParseInput(string[] inputLines)
    {
        _input = inputLines[0];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer("00000");

        return new PuzzleAnswer(answer, 282749);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer("000000");

        return new PuzzleAnswer(answer, 9962624);
    }

    private int GetAnswer(string pattern)
    {
        var queue = new ConcurrentQueue<int>();

        Parallel.ForEach(
            NumbersFrom(1),
            (currentIndex, state) =>
            {
                var hashString = ComputeHash($"{_input}{currentIndex}");
                if (hashString.StartsWith(pattern))
                {
                    queue.Enqueue(currentIndex);
                    state.Stop();
                }
            }
        );

        return queue.Min();
    }

    private static string ComputeHash(string input)
    {
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = MD5.HashData(inputBytes);

        return Convert.ToHexStringLower(hashBytes);
    }

    private static IEnumerable<int> NumbersFrom(int i)
    {
        while (i < int.MaxValue) yield return i++;
    }
}