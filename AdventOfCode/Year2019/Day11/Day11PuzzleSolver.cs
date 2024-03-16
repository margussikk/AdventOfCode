using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.Text;
using AdventOfCode.Year2019.IntCode;
using System.Text;

namespace AdventOfCode.Year2019.Day11;

[Puzzle(2019, 11, "Space Police")]
public class Day11PuzzleSolver : IPuzzleSolver
{
    private IntCodeProgram _program = new();
    public void ParseInput(string[] inputLines)
    {
        _program = IntCodeProgram.Parse(inputLines[0]);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var paintedPanels = PaintHull(Paint.Black);

        var answer = paintedPanels.Count;

        return new PuzzleAnswer(answer, 2064);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var paintedPanels = PaintHull(Paint.White);

        var minRow = int.MaxValue / 2;
        var maxRow = int.MinValue / 2;

        var minColumn = int.MaxValue / 2;
        var maxColumn = int.MinValue / 2;

        foreach (var kvp in paintedPanels.Where(k => k.Value == Paint.White))
        {
            minRow = int.Min(kvp.Key.Row, minRow);
            maxRow = int.Max(kvp.Key.Row, maxRow);

            minColumn = int.Min(kvp.Key.Column, minColumn);
            maxColumn = int.Max(kvp.Key.Column, maxColumn);
        }

        var padding = 5 - (maxColumn - minColumn + 1) % 5; // 5 - small letter width

        var stringBuilder = new StringBuilder();
        for (var row = minRow; row <= maxRow; row++)
        {
            for (var column = minColumn; column <= maxColumn; column++)
            {
                var coordinate = new GridCoordinate(row, column);

                var pixel = paintedPanels.GetValueOrDefault(coordinate, Paint.Black);
                if (pixel == Paint.White)
                {
                    stringBuilder.Append('#');
                }
                else
                {
                    stringBuilder.Append(' ');
                }
            }

            for (var i = 0; i < padding; i++)
            {
                stringBuilder.Append(' ');
            }

            stringBuilder.AppendLine();
        }

        var answer = Ocr.Parse(stringBuilder.ToString());

        return new PuzzleAnswer(answer, "LPZKLGHR");
    }

    private Dictionary<GridCoordinate, long> PaintHull(long startPaint)
    {
        var startCoordinate = new GridCoordinate(0, 0);

        var gridWalker = new GridWalker(startCoordinate, startCoordinate, GridDirection.Up, 0);

        var paintedPanels = new Dictionary<GridCoordinate, long>
        {
            [startCoordinate] = startPaint
        };

        var computer = new IntCodeComputer();
        computer.Load(_program);

        while (true)
        {

            var currentColor = paintedPanels.GetValueOrDefault(gridWalker.CurrentCoordinate, 0);
            computer.Inputs.Enqueue(currentColor);

            var exitCode = computer.Run();
            if (exitCode == IntCodeExitCode.WaitingForInput)
            {
                paintedPanels[gridWalker.CurrentCoordinate] = computer.Outputs.Dequeue();

                var turn = computer.Outputs.Dequeue();
                if (turn == 0)
                {
                    gridWalker.TurnLeft();
                }
                else if (turn == 1)
                {
                    gridWalker.TurnRight();
                }
                else
                {
                    throw new InvalidOperationException($"Invalid turn {turn}");
                }
            }
            else
            {
                break;
            }
        }

        return paintedPanels;
    }
}