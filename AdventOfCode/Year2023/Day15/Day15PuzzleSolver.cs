﻿using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2023.Day15;

[Puzzle(2023, 15, "Lens Library")]
public class Day15PuzzleSolver : IPuzzleSolver
{
    private List<Operation> _operations = [];

    public void ParseInput(string[] inputLines)
    {
        _operations = inputLines[0]
            .Split(',')
            .Select(Operation.Parse)
            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _operations.Sum(o => o.GetHash());

        return new PuzzleAnswer(answer, 517315);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var boxes = Enumerable.Range(1, 256)
            .Select(x => new Box(x))
            .ToList();

        foreach (var operation in _operations)
        {
            var box = boxes[operation.GetLabelHash()];

            switch (operation)
            {
                case RemoveLensOperation removeLensOperation:
                    box.RemoveLens(removeLensOperation.Label);
                    break;
                case ReplaceLensOperation replaceLensOperation:
                    box.ReplaceLens(replaceLensOperation.Label, replaceLensOperation.FocalLength);
                    break;
            }
        }

        var answer = boxes
            .Sum(x => x.CalculateFocalLength());

        return new PuzzleAnswer(answer, 247763);
    }
}
