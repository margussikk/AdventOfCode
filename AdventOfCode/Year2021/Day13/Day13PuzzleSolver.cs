using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.Text;
using System.Text;

namespace AdventOfCode.Year2021.Day13;

[Puzzle(2021, 13, "Transparent Origami")]
public class Day13PuzzleSolver : IPuzzleSolver
{
    private List<Coordinate2D> _coordinates = [];
    private List<FoldInstruction> _foldInstructions = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        _coordinates = chunks[0].Select(Coordinate2D.Parse)
                                .ToList();

        _foldInstructions = chunks[1]
            .Select(line =>
            {
                var splits = line["fold along ".Length..].Split("=");
                return new FoldInstruction((Axis)(splits[0][0] - 'x'), int.Parse(splits[1]));
            })
            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var instruction = _foldInstructions[0];

        var answer = _coordinates
            .Select(x => Fold(x, instruction))
            .Distinct()
            .Count();

        return new PuzzleAnswer(answer, 745);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var coordinates = _coordinates.ToHashSet();

        foreach (var instruction in _foldInstructions)
        {
            coordinates = coordinates
                .Select(x => Fold(x, instruction))
                .ToHashSet();
        }

        // Print it
        var stringBuilder = new StringBuilder();

        // We know the dimensions because it's written using 8 letters and small ocr font
        for (var y = 0; y < 6; y++)
        {
            for (var x = 0; x < 40; x++)
            {
                var coordinate = new Coordinate2D(x, y);
                if (coordinates.Contains(coordinate))
                {
                    stringBuilder.Append('#');
                }
                else
                {
                    stringBuilder.Append(' ');
                }
            }

            stringBuilder.AppendLine();
        }

        var answer = Ocr.Parse(stringBuilder.ToString());

        return new PuzzleAnswer(answer, "ABKJFBGC");
    }

    private static Coordinate2D Fold(Coordinate2D coordinate, FoldInstruction instruction)
    {
        if (instruction.Axis == Axis.X)
        {
            if (coordinate.X < instruction.FoldLine)
            {
                return coordinate;
            }
            else
            {
                return new Coordinate2D(2 * instruction.FoldLine - coordinate.X, coordinate.Y);
            }
        }
        else
        {
            if (coordinate.Y < instruction.FoldLine)
            {
                return coordinate;
            }
            else
            {
                return new Coordinate2D(coordinate.X, 2 * instruction.FoldLine - coordinate.Y);
            }
        }
    }
}