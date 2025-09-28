using AdventOfCode.Framework.Puzzle;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Year2016.Day05;

[Puzzle(2016, 5, "How About a Nice Game of Chess?")]
public class Day05PuzzleSolver : IPuzzleSolver
{
    private const int AnswerLength = 8;
    private const int BatchSize = 16 * 1024;

    private string _doorId = string.Empty;

    public void ParseInput(string[] inputLines)
    {
        _doorId = inputLines[0];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = new char[AnswerLength];
        var filledPositions = 0;

        for (var index = 0; index < int.MaxValue && filledPositions < AnswerLength; index += BatchSize)
        {
            var hashes = GetHashes(index);
            foreach (var hash in hashes)
            {
                if (filledPositions < AnswerLength)
                {
                    answer[filledPositions] = hash[5];
                    filledPositions++;
                }
            }
        }

        return new PuzzleAnswer(new string(answer), "801b56a7");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = new char[AnswerLength];
        var filledPositions = 0;

        for (var index = 0; index < int.MaxValue && filledPositions < AnswerLength; index += BatchSize)
        {
            var hashes = GetHashes(index);
            foreach (var hash in hashes)
            {
                var positionChar = hash[5];
                if (positionChar >= '0' && positionChar <= '7')
                {
                    var position = positionChar - '0';
                    if (answer[position] == '\0')
                    {
                        answer[position] = hash[6];
                        filledPositions++;
                    }
                }
            }
        }

        return new PuzzleAnswer(new string(answer), "424a0197");
    }

    private ParallelQuery<string> GetHashes(int startIndex)
    {
        return Enumerable.Range(startIndex, BatchSize)
                         .AsParallel()
                         .AsOrdered()
                         .Select(item =>
                         {
                             var hash = ComputeHash($"{_doorId}{item}");
                             if (hash.StartsWith("00000"))
                             {
                                 return hash;
                             }
                             return string.Empty;
                         })
                         .Where(x => x != string.Empty);
    }

    private static string ComputeHash(string input)
    {
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = MD5.HashData(inputBytes);

        return Convert.ToHexString(hashBytes).ToLower();
    }
}