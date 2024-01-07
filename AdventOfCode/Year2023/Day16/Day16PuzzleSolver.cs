using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2023.Day16;

[Puzzle(2023, 16, "The Floor Will Be Lava")]
public class Day16PuzzleSolver : IPuzzleSolver
{
    private Grid<Tile> _tiles = new(0, 0);

    public void ParseInput(List<string> inputLines)
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

                // Process beam's movement
                if (tile is Tile.PositiveSlopeMirror or Tile.NegativeSlopeMirror)
                {
                    if ((tile == Tile.PositiveSlopeMirror && (beam.Direction is GridDirection.Left or GridDirection.Right)) ||
                        (tile == Tile.NegativeSlopeMirror && (beam.Direction is GridDirection.Up or GridDirection.Down)))
                    {
                        beam.TurnLeft();
                    }
                    else
                    {
                        beam.TurnRight();
                    }
                }
                else if ((tile is Tile.HorizontalSplitter && (beam.Direction is GridDirection.Up or GridDirection.Down)) ||
                         (tile is Tile.VerticalSplitter && (beam.Direction is GridDirection.Left or GridDirection.Right)))
                {
                    // New beam turns left
                    var clone = beam.Clone();
                    clone.TurnLeft();
                    beams.Push(clone);

                    // Current beam turns right
                    beam.TurnRight();
                }
                else // Empty space or splitter in the passthrough orientation
                {
                    beam.Step();
                }
            }
        }

        return tileVisits.Count(x => x != GridDirection.None);
    }

    private static GridDirection GetVisitedDirection(Tile tile, GridDirection beamDirection)
    {
        if (tile is Tile.EmptySpace or Tile.HorizontalSplitter or Tile.VerticalSplitter)
        {
            if ((beamDirection & (GridDirection.Left | GridDirection.Right)) != GridDirection.None)
            {
                return GridDirection.Left | GridDirection.Right;
            }
            else if ((beamDirection & (GridDirection.Up | GridDirection.Down)) != GridDirection.None)
            {
                return GridDirection.Up | GridDirection.Down;
            }
        }
        else if (tile is Tile.PositiveSlopeMirror)
        {
            if ((beamDirection & (GridDirection.Right | GridDirection.Down)) != GridDirection.None)
            {
                return (GridDirection.Right | GridDirection.Down).Flip();
            }
            else if ((beamDirection & (GridDirection.Left | GridDirection.Up)) != GridDirection.None)
            {
                return (GridDirection.Left | GridDirection.Up).Flip();
            }
        }
        else if (tile is Tile.NegativeSlopeMirror)
        {
            if ((beamDirection & (GridDirection.Right | GridDirection.Up)) != GridDirection.None)
            {
                return (GridDirection.Right | GridDirection.Up).Flip();
            }
            else if ((beamDirection & (GridDirection.Left | GridDirection.Down)) != GridDirection.None)
            {
                return (GridDirection.Left | GridDirection.Down).Flip();
            }
        }

        return GridDirection.None;
    }
}
