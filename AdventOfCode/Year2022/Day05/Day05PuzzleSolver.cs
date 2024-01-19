using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using Day05;

namespace AdventOfCode.Year2022.Day05;

[Puzzle(2022, 5, "Supply Stacks")]
public class Day05PuzzleSolver : IPuzzleSolver
{
    private readonly List<List<char>> _stacks = [];
    private List<Instruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        // Stacks
        var stackCount = chunks[0][^1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        for (var i = 0; i < stackCount; i++)
        {
            _stacks.Add([]);
        }

        foreach (var line in chunks[0])
        {
            for (var stackIndex = 0; stackIndex < _stacks.Count; stackIndex++)
            {
                var cargo = line[stackIndex * 4 + 1];
                if (char.IsAsciiLetter(cargo))
                {
                    _stacks[stackIndex].Add(cargo);
                }
            }
        }

        // Instructions
        _instructions = chunks[1].Select(Instruction.Parse)
                                 .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var stacks = _stacks
            .Select(x => new Stack<char>(x.Reverse<char>()))
            .ToList();

        foreach (var instruction in _instructions)
        {
            for (var i = 0; i < instruction.Quantity; i++)
            {
                var cargo = stacks[instruction.FromStack].Pop();
                stacks[instruction.ToStack].Push(cargo);
            }
        }

        var answer = string.Join(string.Empty, stacks.Select(x => x.Pop()));

        return new PuzzleAnswer(answer, "GRTSWNJHH");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var stacks = _stacks
            .Select(x => new List<char>(x))
            .ToList();

        foreach (var instruction in _instructions)
        {
            var cargos = stacks[instruction.FromStack].Take(instruction.Quantity).ToList();
            stacks[instruction.ToStack].InsertRange(0, cargos);
            stacks[instruction.FromStack].RemoveRange(0, instruction.Quantity);
        }

        var answer = string.Join(string.Empty, stacks.Select(x => x[0]));

        return new PuzzleAnswer(answer, "QLFQDBBHM");
    }
}