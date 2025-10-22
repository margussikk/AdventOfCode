using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Numerics;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Year2016.Day05;

[Puzzle(2016, 5, "How About a Nice Game of Chess?")]
public class Day05PuzzleSolver : IPuzzleSolver
{
    private string _input = string.Empty;

    public void ParseInput(string[] inputLines)
    {
        _input = inputLines[0];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetHashes()
            .Select(x => x[5])
            .Take(8)
            .JoinToString();

        return new PuzzleAnswer(new string(answer), "801b56a7");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = new char[_input.Length];
        var filledPositions = 0;

        foreach (var hash in GetHashes())
        {
            var positionChar = hash[5];
            if (positionChar >= '0' && positionChar <= '7')
            {
                var position = positionChar - '0';
                if (answer[position] == '\0')
                {
                    answer[position] = hash[6];
                    filledPositions++;

                    if (filledPositions == answer.Length)
                    {
                        break;
                    }
                }
            }
        }

        return new PuzzleAnswer(new string(answer), "424a0197");
    }

    private IEnumerable<string> GetHashes()
    {
        var index = 0;
        while (index < int.MaxValue)
        {
            var queue = new ConcurrentQueue<(int Index, string Hash)>();

            Parallel.ForEach(
                NumberGenerator.From(index),
                (currentIndex, state) =>
                {
                    var hashString = ComputeHash($"{_input}{currentIndex}");
                    if (hashString.StartsWith("00000"))
                    {
                        queue.Enqueue((currentIndex, hashString));
                        state.Stop();
                    }
                }
            );

            var item = queue.OrderBy(x => x.Index).First();
            index = item.Index + 1;

            yield return item.Hash;
        }
    }

    private static string ComputeHash(string input)
    {
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = MD5.HashData(inputBytes);

        return Convert.ToHexStringLower(hashBytes);
    }
}