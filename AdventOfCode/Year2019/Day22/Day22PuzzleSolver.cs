using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Mathematics;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spectre.Console.Rendering;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Numerics;

namespace AdventOfCode.Year2019.Day22;

[Puzzle(2019, 22, "Slam Shuffle")]
public class Day22PuzzleSolver : IPuzzleSolver
{
    private List<Technique> _techniques = [];

    public void ParseInput(string[] inputLines)
    {
        _techniques = inputLines.Select(Technique.Parse).ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = 2019L;
        foreach (var technique in _techniques)
        {
            answer = technique.Apply(answer, 10007);
        }

        return new PuzzleAnswer(answer, 3377);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        const long deckSize = 119315717514047L;
        const long times = 101741582076661L;
        
        var reverseTechnique = new ReverseTechnique(deckSize);

        foreach (var technique in _techniques.Reverse<Technique>())
        {
            reverseTechnique.Combine(technique);
        }

        var answer = reverseTechnique.Apply(2020, times);

        return new PuzzleAnswer(answer, 29988879027217L);
    }   
}