using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2020.Day17;

[Puzzle(2020, 17, "Conway Cubes")]
public class Day17PuzzleSolver : IPuzzleSolver
{
    private Grid<bool> _bitGrid = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _bitGrid = inputLines.SelectToGrid(character => character == '#');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        const int totalCycles = 6;

        // Initialize Aabb
        var minCoordinate = new Coordinate3D(-totalCycles, -totalCycles, -totalCycles);
        var maxCoordinate = new Coordinate3D(_bitGrid.LastColumn + totalCycles, _bitGrid.LastRow + totalCycles, totalCycles);
        var aabb = new Aabb3D(minCoordinate, maxCoordinate);

        foreach (var cell in _bitGrid.Where(c => c.Object))
        {
            aabb[cell.Coordinate.Column, cell.Coordinate.Row, 0] = true; // NB! Column is X, row is Y
        }

        // Simulate cycles
        for (var cycle = 1; cycle <= totalCycles; cycle++)
        {
            var updatedCells = new List<Aabb3DCell>();

            var windowMinCoordinate = new Coordinate3D(-cycle, -cycle, -cycle);
            var windowMaxCoordinate = new Coordinate3D(_bitGrid.LastColumn + cycle, _bitGrid.LastRow + cycle, cycle);

            foreach (var cell in aabb.Window(windowMinCoordinate, windowMaxCoordinate))
            {
                var activeNeighborsCount = aabb.AroundNeighbors(cell.Coordinate)
                                               .Count(c => c.Active);

                if (cell.Active)
                {
                    if (activeNeighborsCount != 2 && activeNeighborsCount != 3)
                    {
                        updatedCells.Add(new Aabb3DCell(cell.Coordinate, false));
                    }
                }
                else
                {
                    if (activeNeighborsCount == 3)
                    {
                        updatedCells.Add(new Aabb3DCell(cell.Coordinate, true));
                    }
                }
            }

            foreach (var updatedCell in updatedCells)
            {
                aabb[updatedCell.Coordinate] = updatedCell.Active;
            }
        }

        var answer = aabb.Count(cell => cell.Active);

        return new PuzzleAnswer(answer, 215);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        const int totalCycles = 6;

        // Initialize Aabb
        var minCoordinate = new Coordinate4D(-totalCycles, -totalCycles, -totalCycles, -totalCycles);
        var maxCoordinate = new Coordinate4D(_bitGrid.LastColumn + totalCycles, _bitGrid.LastRow + totalCycles, totalCycles, totalCycles);
        var aabb = new Aabb4D(minCoordinate, maxCoordinate);

        foreach (var cell in _bitGrid.Where(c => c.Object))
        {
            aabb[cell.Coordinate.Column, cell.Coordinate.Row, 0, 0] = true; // NB! Column is X, row is Y
        }

        // Simulate cycles
        for (var cycle = 1; cycle <= totalCycles; cycle++)
        {
            var updatedCells = new List<Aabb4DCell>();

            var windowMinCoordinate = new Coordinate4D(-cycle, -cycle, -cycle, -cycle);
            var windowMaxCoordinate = new Coordinate4D(_bitGrid.LastColumn + cycle, _bitGrid.LastRow + cycle, cycle, cycle);

            foreach (var cell in aabb.Window(windowMinCoordinate, windowMaxCoordinate))
            {
                var activeNeighborsCount = aabb.AroundNeighbors(cell.Coordinate)
                                                .Count(c => c.Active);

                if (cell.Active)
                {
                    if (activeNeighborsCount != 2 && activeNeighborsCount != 3)
                    {
                        updatedCells.Add(new Aabb4DCell(cell.Coordinate, false));
                    }
                }
                else
                {
                    if (activeNeighborsCount == 3)
                    {
                        updatedCells.Add(new Aabb4DCell(cell.Coordinate, true));
                    }
                }
            }

            foreach (var updatedCell in updatedCells)
            {
                aabb[updatedCell.Coordinate] = updatedCell.Active;
            }
        }

        var answer = aabb.Count(cell => cell.Active);

        return new PuzzleAnswer(answer, 1728);
    }
}