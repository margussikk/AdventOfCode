using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.PathFinding;
using System.Text;

namespace AdventOfCode.Year2024.Day21;
internal class KeypadRobot
{
    private readonly Dictionary<(char, char), List<string>> _sequencesCache;
    private readonly Dictionary<(char, char), long> _subLengthsCache = [];
    private readonly Grid<char> _keypad;

    public KeypadRobot? ControlledBy { get; set; }

    public KeypadRobot(Dictionary<(char, char), List<string>> sequencesCache, bool isNumericKeypadRobot)
    {
        _sequencesCache = sequencesCache;
        _keypad = isNumericKeypadRobot ? BuildNumericKeypad() : BuildDirectionalKeypad();
    }

    public long CalculateLength(string code, int level)
    {
        var length = 0L;

        var current = 'A';
        foreach (var next in code)
        {
            var cacheKey = (current, next);
            if (!_subLengthsCache.TryGetValue(cacheKey, out var subLength))
            {
                if (ControlledBy == null)
                {
                    subLength = GetSequences(current, next)[0].Length; // These all have the same length
                }
                else
                {
                    subLength = GetSequences(current, next).Min(sequence => ControlledBy.CalculateLength(sequence, level + 1));
                }

                _subLengthsCache[cacheKey] = subLength;
            }

            length += subLength;

            current = next;
        }

        return length;
    }

    public string PushButtons(string directions)
    {
        var coordinate = _keypad.First(cell => cell.Object == 'A').Coordinate;

        var stringBuilder = new StringBuilder();

        foreach (var direction in directions)
        {
            if (direction == 'A')
            {
                stringBuilder.Append(_keypad[coordinate]);
            }
            else
            {
                coordinate = coordinate.Move(direction.ParseToGridDirection());
            }
        }

        return stringBuilder.ToString();
    }

    public List<string> GetSequences(char startButton, char endButton)
    {
        var key = (startButton, endButton);

        if (!_sequencesCache.TryGetValue(key, out var sequences))
        {
            var startCoordinate = _keypad.First(cell => cell.Object == startButton).Coordinate;
            var endCoordinate = _keypad.First(cell => cell.Object == endButton).Coordinate;

            var gridPathFinder = new GridPathFinder<char>(_keypad)
                .SetCellFilter((_, c) => c.Object != ' ');

            var pathList = gridPathFinder.FindAllShortestPaths(startCoordinate, endCoordinate);

            sequences = [];
            foreach (var path in pathList)
            {
                var directions = new StringBuilder();

                for (var i = 0; i < path.Count - 1; i++)
                {
                    var direction = path[i].DirectionToward(path[i + 1]);

                    var character = direction switch
                    {
                        GridDirection.Up => '^',
                        GridDirection.Down => 'v',
                        GridDirection.Left => '<',
                        GridDirection.Right => '>',
                        _ => throw new NotImplementedException()
                    };

                    directions.Append(character);
                }

                directions.Append('A');

                sequences.Add(directions.ToString());
            }

            _sequencesCache[key] = sequences;
        }

        return sequences;
    }

    private static Grid<char> BuildDirectionalKeypad()
    {
        var keypad = new Grid<char>(2, 3);
        keypad[0, 0] = ' '; // Gap
        keypad[0, 1] = '^';
        keypad[0, 2] = 'A';
        keypad[1, 0] = '<';
        keypad[1, 1] = 'v';
        keypad[1, 2] = '>';

        return keypad;
    }

    private static Grid<char> BuildNumericKeypad()
    {
        var keypad = new Grid<char>(4, 3);
        keypad[0, 0] = '7';
        keypad[0, 1] = '8';
        keypad[0, 2] = '9';
        keypad[1, 0] = '4';
        keypad[1, 1] = '5';
        keypad[1, 2] = '6';
        keypad[2, 0] = '1';
        keypad[2, 1] = '2';
        keypad[2, 2] = '3';
        keypad[3, 0] = ' '; // Gap
        keypad[3, 1] = '0';
        keypad[3, 2] = 'A';

        return keypad;
    }
}
