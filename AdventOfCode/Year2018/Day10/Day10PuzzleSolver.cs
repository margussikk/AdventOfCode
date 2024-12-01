using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.Text;
using System.Text;

namespace AdventOfCode.Year2018.Day10;

[Puzzle(2018, 10, "The Stars Align")]
public class Day10PuzzleSolver : IPuzzleSolver
{
    private List<PointOfLight> _pointsOfLight = [];

    public void ParseInput(string[] inputLines)
    {
        _pointsOfLight = inputLines.Select(PointOfLight.Parse)
                                   .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        (var answer, _) = DetectMessage();

        return new PuzzleAnswer(answer, "BHPJGLPE");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        (_, var answer) = DetectMessage();

        return new PuzzleAnswer(answer, 10831);
    }

    public (string message, int minutes) DetectMessage()
    {
        var pointsOfLight = _pointsOfLight
            .Select(x => new PointOfLight
            {
                Position = x.Position,
                Velocity = x.Velocity,
            })
            .ToList();

        for (var minute = 0; minute < int.MaxValue; minute++)
        {
            var minY = pointsOfLight.Min(p => p.Position.Y);
            var maxY = pointsOfLight.Max(p => p.Position.Y);

            if (maxY - minY + 1 == Ocr.LargeLetterHeight)
            {
                var minX = pointsOfLight.Min(p => p.Position.X);
                var maxX = pointsOfLight.Max(p => p.Position.X);

                var grid = new BitGrid((int)(maxY - minY) + 1, (int)(maxX - minX) + 1);

                foreach (var pointOfLight in pointsOfLight)
                {
                    grid[(int)(pointOfLight.Position.Y - minY), (int)(pointOfLight.Position.X - minX)] = true;
                }

                var stringBuilder = new StringBuilder();

                var padding = 8 - grid.Width % 8; // 8 - large letter width

                for (var row = 0; row <= grid.LastRowIndex; row++)
                {
                    for (var column = 0; column <= grid.LastColumnIndex; column++)
                    {
                        var character = grid[row, column] ? '#' : ' ';
                        stringBuilder.Append(character);
                    }

                    for (var i = 0; i < padding; i++)
                    {
                        stringBuilder.Append(' ');
                    }

                    stringBuilder.AppendLine();
                }

                var message = Ocr.Parse(stringBuilder.ToString());

                return (message, minute);
            }

            foreach (var pointOfLight in pointsOfLight)
            {
                pointOfLight.Move();
            }
        }

        return (string.Empty, int.MaxValue);
    }
}