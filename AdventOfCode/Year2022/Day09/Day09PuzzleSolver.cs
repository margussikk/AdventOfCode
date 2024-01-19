using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2022.Day09;

[Puzzle(2022, 9, "Rope Bridge")]
public class Day09PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Instruction> _instructions = Array.Empty<Instruction>();

    public void ParseInput(string[] inputLines)
    {
        _instructions = inputLines.Select(Instruction.Parse)
                                  .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = CountTailPositions(2);

        return new PuzzleAnswer(answer, 5902);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = CountTailPositions(10);

        return new PuzzleAnswer(answer, 2445);
    }


    private int CountTailPositions(int knotCount)
    {
        var tailCoordinates = new HashSet<GridCoordinate>();

        var knotCoordinates = new GridCoordinate[knotCount];
        for (var i = 0; i < knotCount; i++)
        {
            knotCoordinates[i] = new GridCoordinate(0, 0);
        }

        tailCoordinates.Add(knotCoordinates[knotCount - 1]);

        foreach (var instruction in _instructions)
        {
            for (var step = 0; step < instruction.Steps; step++)
            {
                // Move head
                knotCoordinates[0] = knotCoordinates[0].Move(instruction.Direction);

                // Move every other knot
                for (var knotIndex = 1; knotIndex < knotCount; knotIndex++)
                {
                    var rowDistance = knotCoordinates[knotIndex - 1].Row - knotCoordinates[knotIndex].Row;
                    var columnDistance = knotCoordinates[knotIndex - 1].Column - knotCoordinates[knotIndex].Column;

                    if (Math.Abs(rowDistance) > 1 || Math.Abs(columnDistance) > 1)
                    {
                        var rowStep = Math.Sign(rowDistance);
                        var columnStep = Math.Sign(columnDistance);

                        knotCoordinates[knotIndex] = new GridCoordinate(knotCoordinates[knotIndex].Row + rowStep, knotCoordinates[knotIndex].Column + columnStep);

                        if (knotIndex == knotCount - 1)
                        {
                            tailCoordinates.Add(knotCoordinates[knotIndex]);
                        }
                    }
                }
            }
        }

        return tailCoordinates.Count;
    }
}