using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.Mathematics;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2022.Day22;

internal class CubeWalker
{
    public GridDirection Direction { get; private set; }

    private readonly Grid<Tile> _grid;

    private readonly Dictionary<CubeFace, GridCoordinate> _cubeFaceLocationMapping = [];

    private readonly Dictionary<CubeFace, int> _cubeFaceOffset = [];

    private CubeFace _cubeFace;

    private GridCoordinate _faceLocation;

    private readonly int _cubeEdgeLength;

    public CubeWalker(Grid<Tile> grid)
    {
        _grid = grid;
        _cubeEdgeLength = int.Max(_grid.Height, _grid.Width) % int.Min(_grid.Height, _grid.Width);

        // Find start location
        var coordinate = _grid.FindCoordinate(tile => tile == Tile.Open) ?? throw new InvalidOperationException("Flat walker stat location not found");
        _faceLocation = new GridCoordinate(coordinate.Row % _cubeEdgeLength, coordinate.Column % _cubeEdgeLength);
        _cubeFace = CubeFace.Front;
        Direction = GridDirection.Right;

        ConstructCubeParameters();
    }

    private void ConstructCubeParameters()
    {
        // Find cube face locations
        var cubeFaceLocations = new HashSet<GridCoordinate>();
        for (var row = 0; row < _grid.Height; row += _cubeEdgeLength)
        {
            for (var column = 0; column < _grid.Width; column += _cubeEdgeLength)
            {
                if (_grid[row, column] != Tile.Nothing)
                {
                    cubeFaceLocations.Add(new GridCoordinate(row / _cubeEdgeLength, column / _cubeEdgeLength));
                }
            }
        }

        // Find cube layout
        var queue = new Queue<(GridCoordinate Location, CubeFace Face, GridDirection Direction, CubeFace FromFace)>();
        var visited = new HashSet<GridCoordinate>();

        queue.Enqueue((cubeFaceLocations.First(), CubeFace.Front, GridDirection.Down, CubeFace.Top));
        while (queue.TryDequeue(out var current))
        {
            if (!visited.Add(current.Location))
            {
                continue;
            }

            _cubeFaceLocationMapping[current.Face] = current.Location;

            // Find the reverse of incoming direction
            var reverseDirectionIndex = Array.IndexOf(_directions, current.Direction.Flip());
            var neighborIndex = Array.IndexOf(_cubeFaceNeighbors[current.Face], current.FromFace);

            // Offset = difference of what it would be if the faces touch and what it actually is, Essentially it shows how many times
            // it needs to be rotated so that the left and right match up and up and down match up.

            //  1    If going from 1 to 2, then 2 expects an entry from its right,
            // 234   but entry came from top, top neighbor is left's 3rd neighbor, so offset is 3
            //  5
            //  6

            var offset = MathFunctions.Modulo(reverseDirectionIndex - neighborIndex, 4);
            _cubeFaceOffset[current.Face] = offset;

            // Find neighbors in all 4 directions
            foreach (var direction in _directions)
            {
                var location = current.Location.Move(direction);
                if (!cubeFaceLocations.Contains(location)) continue;

                var directionIndex = Array.IndexOf(_directions, direction);
                var newFaceIndex = MathFunctions.Modulo(directionIndex - offset, 4);
                var newFace = _cubeFaceNeighbors[current.Face][newFaceIndex];

                queue.Enqueue((location, newFace, direction, current.Face));
            }
        }
    }

    public int GetGridRow()
    {
        return _cubeFaceLocationMapping[_cubeFace].Row * _cubeEdgeLength + _faceLocation.Row;
    }

    public int GetGridColumn()
    {
        return _cubeFaceLocationMapping[_cubeFace].Column * _cubeEdgeLength + _faceLocation.Column;
    }

    public void TurnLeft()
    {
        Direction = Direction.TurnLeft();
    }

    public void TurnRight()
    {
        Direction = Direction.TurnRight();
    }

    public void Move(int steps)
    {
        var edgeRange = new NumberRange<int>(0, _cubeEdgeLength - 1);

        for (var step = 0; step < steps; step++)
        {
            var testLocation = _faceLocation.Move(Direction);
            var testCubeFace = _cubeFace;
            var testDirection = Direction;

            if (!edgeRange.Contains(testLocation.Row) || !edgeRange.Contains(testLocation.Column))
            {
                // Normalize location
                var normalizedRow = MathFunctions.Modulo(testLocation.Row, _cubeEdgeLength);
                var normalizedColumn = MathFunctions.Modulo(testLocation.Column, _cubeEdgeLength);
                testLocation = new GridCoordinate(normalizedRow, normalizedColumn);

                // Find face which is at the 'Direction' side
                var directionIndex = Array.IndexOf(_directions, Direction);
                var testCubeFaceIndex = MathFunctions.Modulo(directionIndex - _cubeFaceOffset[_cubeFace], 4);
                testCubeFace = _cubeFaceNeighbors[_cubeFace][testCubeFaceIndex];

                // Rotate testlocation
                var neighborIndex = Array.IndexOf(_cubeFaceNeighbors[testCubeFace], _cubeFace);
                var reverseDirectionIndex = Array.IndexOf(_directions, Direction.Flip());
                var positionOffset = MathFunctions.Modulo(neighborIndex - reverseDirectionIndex, 4);
                var offset = _cubeFaceOffset[testCubeFace];
                var rotations = (positionOffset + offset) % 4;

                for (var i = 0; i < rotations; i++)
                {
                    testDirection = testDirection.TurnRight();
                    testLocation = new GridCoordinate(testLocation.Column, _cubeEdgeLength - 1 - testLocation.Row);
                }
            }

            var testGridRow = _cubeFaceLocationMapping[testCubeFace].Row * _cubeEdgeLength + testLocation.Row;
            var testGridColumn = _cubeFaceLocationMapping[testCubeFace].Column * _cubeEdgeLength + testLocation.Column;

            switch (_grid[testGridRow, testGridColumn])
            {
                case Tile.Wall:
                    return;
                case Tile.Open:
                    _faceLocation = testLocation;
                    _cubeFace = testCubeFace;
                    Direction = testDirection;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }

    public void Print()
    {
        Console.WriteLine($"Face: {_cubeFace}, Row: {_faceLocation.Row}, Column: {_faceLocation.Column}, Direction: {Direction}");
    }

    private static readonly GridDirection[] _directions =
    [
        GridDirection.Right,
        GridDirection.Down,
        GridDirection.Left,
        GridDirection.Up
    ];

    private static readonly IReadOnlyDictionary<CubeFace, CubeFace[]> _cubeFaceNeighbors = new Dictionary<CubeFace, CubeFace[]>
    {
        // Array items are ordered by going clockwise order starting from 'Right' direction.
        [CubeFace.Front] = [CubeFace.Right, CubeFace.Bottom, CubeFace.Left, CubeFace.Top],
        [CubeFace.Back] = [CubeFace.Left, CubeFace.Bottom, CubeFace.Right, CubeFace.Top],
        [CubeFace.Left] = [CubeFace.Front, CubeFace.Bottom, CubeFace.Back, CubeFace.Top],
        [CubeFace.Right] = [CubeFace.Back, CubeFace.Bottom, CubeFace.Front, CubeFace.Top],
        [CubeFace.Top] = [CubeFace.Right, CubeFace.Front, CubeFace.Left, CubeFace.Back],
        [CubeFace.Bottom] = [CubeFace.Right, CubeFace.Back, CubeFace.Left, CubeFace.Front]
    };
}
