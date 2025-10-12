using AdventOfCode.Framework.Puzzle;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Year2016.Day14;

[Puzzle(2016, 14, "One-Time Pad")]
public class Day14PuzzleSolver : IPuzzleSolver
{
    private string _input = string.Empty;

    public void ParseInput(string[] inputLines)
    {
        _input = inputLines[0];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(0);

        return new PuzzleAnswer(answer, 16106);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(2016);

        return new PuzzleAnswer(answer, 22423);
    }

    private int GetAnswer(int stretches)
    {
        var keyInfos = new List<KeyInfo>();

        var answerKeyInfos = new List<KeyInfo>();

        var input = _input;

        var batchSize = 1024;

        for (var startIndex = 0; startIndex < int.MaxValue; startIndex += batchSize)
        {
            var hashes = Enumerable.Range(startIndex, batchSize)
                                   .AsParallel()
                                   .AsOrdered()
                                   .Select(index =>
                                   {
                                       var hash = $"{input}{index}";
                                       for (var i = 0; i < stretches + 1; i++)
                                       {
                                           hash = ComputeHash(hash);
                                       }

                                       return (Index: index, Hash: hash);
                                   });

            foreach (var (itemIndex, hash) in hashes)
            {
                var tripletChar = FindSequenceChar(hash, 3);
                if (tripletChar != '\0')
                {
                    keyInfos.Add(new KeyInfo { Character = tripletChar, Index = itemIndex });

                    var quintupleChar = FindSequenceChar(hash, 5);
                    if (quintupleChar != '\0')
                    {
                        foreach (var keyInfo in keyInfos.Where(keyInfo => keyInfo.Character == quintupleChar && keyInfo.Index < itemIndex && keyInfo.Index >= itemIndex - 1000))
                        {
                            keyInfo.IsKey = true;
                        }
                    }
                }

                if (keyInfos.Count != 0 && keyInfos[0].Index < itemIndex - 1000)
                {
                    if (keyInfos[0].IsKey)
                    {
                        answerKeyInfos.Add(keyInfos[0]);
                        if (answerKeyInfos.Count == 64)
                        {
                            break;
                        }
                    }
                    keyInfos.RemoveAt(0);
                }
            }

            if (answerKeyInfos.Count == 64)
            {
                break;
            }
        }

        return answerKeyInfos[^1].Index;
    }

    private static string ComputeHash(string input)
    {
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = MD5.HashData(inputBytes);

        return Convert.ToHexStringLower(hashBytes);
    }

    private static char FindSequenceChar(ReadOnlySpan<char> span, int length)
    {
        for (var i = 0; i < span.Length - length + 1; i++)
        {
            var firstChar = span[i];

            for (var j = 1; j < length; j++)
            {
                if (span[i + j] != firstChar)
                {
                    firstChar = '\0';
                    break;
                }
            }

            if (firstChar != '\0')
            {
                return firstChar;
            }
        }

        return '\0';
    }

    private sealed class KeyInfo
    {
        public char Character { get; set; } = '\0';
        public int Index { get; set; }
        public bool IsKey { get; set; }
    }
}