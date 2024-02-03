using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;
using BenchmarkDotNet.Columns;

namespace AdventOfCode.Year2020.Day17;

[Puzzle(2020, 17, "Conway Cubes")]
public class Day17PuzzleSolver : IPuzzleSolver
{
    private BitGrid _bitGrid = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _bitGrid = inputLines.SelectToBitGrid(character => character == '#');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var totalCycles = 6;

        // Initialize Aabb
        var minCoordinate = new Coordinate3D(-totalCycles, -totalCycles, -totalCycles);
        var maxCoordinate = new Coordinate3D(_bitGrid.LastColumnIndex + totalCycles, _bitGrid.LastRowIndex + totalCycles, totalCycles);
        var aabb = new Aabb3D(minCoordinate, maxCoordinate);

        foreach(var cell in _bitGrid.Where(c => c.Object))
        {
            aabb[cell.Coordinate.Column, cell.Coordinate.Row, 0] = true; // NB! Column is X, row is Y
        }

        // Simulate cycles
        for (var cycle = 1; cycle <= totalCycles; cycle++)
        {
            var windowSize = totalCycles - cycle;
            var updatedCells = new List<Aabb3DCell>();

            for (var z = aabb.MinCoordinate.Z + windowSize; z <= aabb.MaxCoordinate.Z - windowSize; z++)
            {
                for (var y = aabb.MinCoordinate.Y + windowSize; y <= aabb.MaxCoordinate.Y - windowSize; y++)
                {
                    for (var x = aabb.MinCoordinate.X + windowSize; x <= aabb.MaxCoordinate.X - windowSize; x++)
                    {
                        var coordinate = new Coordinate3D(x, y, z);
                        var activeNeighborsCount = aabb.AroundNeighbors(coordinate)
                                                       .Count(c => c.Active);
                        var active = aabb[coordinate];
                        if (active)
                        {
                            if (activeNeighborsCount != 2 && activeNeighborsCount != 3)
                            {
                                updatedCells.Add(new Aabb3DCell(coordinate, false));
                            }
                        }
                        else
                        {
                            if (activeNeighborsCount == 3)
                            {
                                updatedCells.Add(new Aabb3DCell(coordinate, true));
                            }
                        }
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
        var totalCycles = 6;

        // Initialize Aabb
        var minCoordinate = new Coordinate4D(-totalCycles, -totalCycles, -totalCycles, -totalCycles);
        var maxCoordinate = new Coordinate4D(_bitGrid.LastColumnIndex + totalCycles, _bitGrid.LastRowIndex + totalCycles, totalCycles, totalCycles);
        var aabb = new Aabb4D(minCoordinate, maxCoordinate);

        foreach (var cell in _bitGrid.Where(c => c.Object))
        {
            aabb[cell.Coordinate.Column, cell.Coordinate.Row, 0, 0] = true; // NB! Column is X, row is Y
        }

        // Simulate cycles
        for (var cycle = 1; cycle <= totalCycles; cycle++)
        {
            var windowSize = totalCycles - cycle;
            var updatedCells = new List<Aabb4DCell>();

            for (var w = aabb.MinCoordinate.W + windowSize; w <= aabb.MaxCoordinate.W - windowSize; w++)
            {
                for (var z = aabb.MinCoordinate.Z + windowSize; z <= aabb.MaxCoordinate.Z - windowSize; z++)
                {
                    for (var y = aabb.MinCoordinate.Y + windowSize; y <= aabb.MaxCoordinate.Y - windowSize; y++)
                    {
                        for (var x = aabb.MinCoordinate.X + windowSize; x <= aabb.MaxCoordinate.X - windowSize; x++)
                        {
                            var coordinate = new Coordinate4D(x, y, z, w);
                            var activeNeighborsCount = aabb.AroundNeighbors(coordinate)
                                                           .Count(c => c.Active);
                            var active = aabb[coordinate];
                            if (active)
                            {
                                if (activeNeighborsCount != 2 && activeNeighborsCount != 3)
                                {
                                    updatedCells.Add(new Aabb4DCell(coordinate, false));
                                }
                            }
                            else
                            {
                                if (activeNeighborsCount == 3)
                                {
                                    updatedCells.Add(new Aabb4DCell(coordinate, true));
                                }
                            }
                        }
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