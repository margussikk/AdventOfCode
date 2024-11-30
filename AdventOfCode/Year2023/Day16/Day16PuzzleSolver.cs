using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2023.Day16;

[Puzzle(2023, 16, "The Floor Will Be Lava")]
public class Day16PuzzleSolver : IPuzzleSolver
{
    private Grid<Tile> _tiles = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _tiles = inputLines.SelectToGrid(character => character switch
        {
            '.' => Tile.EmptySpace,
            '/' => Tile.PositiveSlopeMirror,
            '\\' => Tile.NegativeSlopeMirror,
            '-' => Tile.HorizontalSplitter,
            '|' => Tile.VerticalSplitter,
            _ => throw new InvalidOperationException()
        });
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var startCoordinate = new GridCoordinate(0, 0);

        var beam = new GridWalker(startCoordinate, startCoordinate, GridDirection.Right, 0);

        var answer = CountEnergizedTiles(beam);

        return new PuzzleAnswer(answer, 7870);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var energizedTiles = new List<int>();

        for (var i = 0; i < _tiles.Height; i++) // NB! Assume that the puzzle input is a square and Height == Width
        {
            // Right
            var rightCoordinate = new GridCoordinate(i, 0);
            var rightBeam = new GridWalker(rightCoordinate, rightCoordinate, GridDirection.Right, 0);
            energizedTiles.Add(CountEnergizedTiles(rightBeam));

            // Left
            var leftCoordinate = new GridCoordinate(i, _tiles.LastColumnIndex);
            var leftBeam = new GridWalker(leftCoordinate, leftCoordinate, GridDirection.Left, 0);
            energizedTiles.Add(CountEnergizedTiles(leftBeam));

            // Down
            var downCoordinate = new GridCoordinate(0, i);
            var downBeam = new GridWalker(downCoordinate, downCoordinate, GridDirection.Down, 0);
            energizedTiles.Add(CountEnergizedTiles(downBeam));

            // Up
            var upCoordinate = new GridCoordinate(_tiles.LastRowIndex, i);
            var upBeam = new GridWalker(upCoordinate, upCoordinate, GridDirection.Up, 0);
            energizedTiles.Add(CountEnergizedTiles(upBeam));
        }

        var answer = energizedTiles.Max();

        return new PuzzleAnswer(answer, 8143);
    }

    private int CountEnergizedTiles(GridWalker beam)
    {
        var tileVisits = new Grid<GridDirection>(_tiles.Height, _tiles.Width);
        var beams = new Stack<GridWalker>();

        beams.Push(beam);
        while (beams.Count > 0)
        {
            beam = beams.Pop();

            while (_tiles.InBounds(beam.CurrentCoordinate))
            {
                var tile = _tiles[beam.CurrentCoordinate];

                // Check visited
                var visitedDirection = GetVisitedDirection(tile, beam.Direction);
                if (tileVisits[beam.CurrentCoordinate].HasFlag(visitedDirection))
                {
                    // Already visited the tile in that general direction (i.e. left to right is the same as right to left)
                    break;
                }

                tileVisits[beam.CurrentCoordinate] |= visitedDirection;

                switch (tile)
                {
                    // Process beam's movement
                    case Tile.PositiveSlopeMirror or Tile.NegativeSlopeMirror when (tile == Tile.PositiveSlopeMirror && beam.Direction is GridDirection.Left or GridDirection.Right) ||
                        (tile == Tile.NegativeSlopeMirror && beam.Direction is GridDirection.Up or GridDirection.Down):
                        beam.TurnLeft();
                        break;
                    case Tile.PositiveSlopeMirror or Tile.NegativeSlopeMirror:
                        beam.TurnRight();
                        break;
                    case Tile.HorizontalSplitter when beam.Direction is GridDirection.Up or GridDirection.Down:
                    case Tile.VerticalSplitter when beam.Direction is GridDirection.Left or GridDirection.Right:
                        {
                            // New beam turns left
                            var clone = beam.Clone();
                            clone.TurnLeft();
                            beams.Push(clone);

                            // Current beam turns right
                            beam.TurnRight();
                            break;
                        }
                    // Empty space or splitter in the passthrough orientation
                    default:
                        beam.Step();
                        break;
                }
            }
        }

        return tileVisits.Count(cell => cell.Object != GridDirection.None);
    }

    private static GridDirection GetVisitedDirection(Tile tile, GridDirection beamDirection)
    {
        return tile switch
        {
            Tile.EmptySpace or
            Tile.HorizontalSplitter or
            Tile.VerticalSplitter when (beamDirection & GridDirection.LeftAndRight) != GridDirection.None
                => GridDirection.LeftAndRight,
            Tile.EmptySpace or
            Tile.HorizontalSplitter or
            Tile.VerticalSplitter when (beamDirection & GridDirection.UpAndDown) != GridDirection.None
                => GridDirection.UpAndDown,
            Tile.PositiveSlopeMirror when (beamDirection & GridDirection.DownAndRight) != GridDirection.None
                => GridDirection.DownAndRight.Flip(),
            Tile.PositiveSlopeMirror when (beamDirection & GridDirection.UpAndLeft) != GridDirection.None
                => GridDirection.UpAndLeft.Flip(),
            Tile.NegativeSlopeMirror when (beamDirection & GridDirection.UpAndRight) != GridDirection.None
                => GridDirection.UpAndRight.Flip(),
            Tile.NegativeSlopeMirror when (beamDirection & GridDirection.DownAndLeft) != GridDirection.None
                => GridDirection.DownAndLeft.Flip(),
            _ => GridDirection.None
        };
    }
}
