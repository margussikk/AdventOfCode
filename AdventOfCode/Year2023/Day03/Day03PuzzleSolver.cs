using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2023.Day03;

[Puzzle(2023, 3, "Gear Ratios")]
public class Day03PuzzleSolver : IPuzzleSolver
{
    private readonly List<EnginePart> _engineParts = [];

    public void ParseInput(string[] inputLines)
    {
        for (var row = 0; row < inputLines.Length; row++)
        {
            var rowSpan = inputLines[row].AsSpan();
            var startColumn = 0;

            while (startColumn < inputLines[row].Length)
            {
                var relativeStartIndex = rowSpan[startColumn..].IndexOfAnyExcept('.');
                if (relativeStartIndex < 0) // End of line
                {
                    break;
                }

                startColumn += relativeStartIndex;

                int endColumn;
                if (char.IsDigit(rowSpan[startColumn]))
                {
                    var digitsEndIndex = rowSpan[startColumn..].IndexOfAnyExceptInRange('0', '9');
                    if (digitsEndIndex < 0) // End of line
                    {
                        endColumn = rowSpan.Length;
                    }
                    else
                    {
                        endColumn = startColumn + digitsEndIndex;
                    }

                    var length = endColumn - startColumn;
                    var number = long.Parse(rowSpan[startColumn..endColumn]);

                    var numberPart = new NumberPart(row, startColumn, length, number);
                    _engineParts.Add(numberPart);
                }
                else
                {
                    endColumn = startColumn + 1; // Assume symbol parts are only one character in length

                    var symbolPart = new SymbolPart(row, startColumn, 1, rowSpan[startColumn]);
                    _engineParts.Add(symbolPart);
                }

                startColumn = endColumn;
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var numberParts = _engineParts
            .OfType<NumberPart>()
            .ToList();

        var symbolParts = _engineParts
            .OfType<SymbolPart>()
            .ToList();

        var answer = numberParts
            .Where(numberPart => symbolParts
                .Exists(symbolPart => numberPart.IsAdjacentTo(symbolPart)))
            .Sum(numberPart => numberPart.Number);

        return new PuzzleAnswer(answer, 527144);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var numberParts = _engineParts
            .OfType<NumberPart>()
            .ToList();

        var gearParts = _engineParts
            .OfType<SymbolPart>()
            .Where(symbolPart => symbolPart.IsGearPart)
            .ToList();

        var answer = gearParts
            .Select(gearPart => numberParts
                .Where(numberPart => numberPart.IsAdjacentTo(gearPart))
                .ToList())
            .Where(adjacentNumberParts => adjacentNumberParts.Count == 2)
            .Sum(adjacentNumberParts => adjacentNumberParts[0].Number * adjacentNumberParts[1].Number);

        return new PuzzleAnswer(answer, 81463996);
    }
}
