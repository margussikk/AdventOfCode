using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.Text;
using System.Text;

namespace AdventOfCode.Year2019.Day08;

[Puzzle(2019, 8, "Space Image Format")]
public class Day08PuzzleSolver : IPuzzleSolver
{
    private readonly List<Grid<Pixel>> _layers = [];

    public void ParseInput(string[] inputLines)
    {
        var characterIndex = 0;

        while (characterIndex < inputLines[0].Length)
        {
            var layer = new Grid<Pixel>(6, 25);

            for (var row = 0; row <= layer.LastRowIndex; row++)
            {
                for (var column = 0; column <= layer.LastColumnIndex; column++)
                {
                    layer[row, column] = (Pixel)(inputLines[0][characterIndex] - '0');
                    characterIndex++;
                }
            }

            _layers.Add(layer);
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var leastTransparentLayer = _layers.MinBy(layer => layer.Count(cell => cell.Object == Pixel.Black))
            ?? throw new InvalidOperationException("Layer not found");

        var ones = leastTransparentLayer.Count(cell => cell.Object == Pixel.White);
        var twos = leastTransparentLayer.Count(cell => cell.Object == Pixel.Transparent);

        var answer = ones * twos;

        return new PuzzleAnswer(answer, 2500);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var stringBuilder = new StringBuilder();
        
        for (var row = 0; row <= _layers[0].LastRowIndex; row++)
        {
            for(var column = 0; column <= _layers[0].LastColumnIndex; column++)
            {
                var layer = _layers.Find(l => l[row, column] != Pixel.Transparent);
                if (layer != null)
                {
                    var character = layer[row, column] == Pixel.White ? '#' : ' ';
                    stringBuilder.Append(character);
                }
            }
            stringBuilder.AppendLine();
        }

        var answer = Ocr.Parse(stringBuilder.ToString());

        return new PuzzleAnswer(answer, "CYUAH");
    }
}