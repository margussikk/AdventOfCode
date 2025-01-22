using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2017.Day22;

[Puzzle(2017, 22, "Sporifica Virus")]
public class Day22PuzzleSolver : IPuzzleSolver
{
    private Grid<State> _grid = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _grid = inputLines.SelectToGrid(c => c == '#' ? State.Infected : State.Clean);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = 0;

        var grid = new InfiniteGrid<State>();

        grid.CopyFrom(_grid, new GridCoordinate(-_grid.Width / 2, -_grid.Height / 2));

        var virus = new GridWalker(new GridPosition(GridCoordinate.Zero, GridDirection.Up));

        for (var burst = 0; burst < 10000; burst++)
        {
            if (grid[virus.Coordinate] == State.Infected)
            {
                virus.TurnRight();
                grid[virus.Coordinate] = State.Clean;
            }
            else
            {
                answer++;
                virus.TurnLeft();
                grid[virus.Coordinate] = State.Infected;
            }

            virus.Step();
        }

        return new PuzzleAnswer(answer, 5450);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0;

        var grid = new InfiniteGrid<State>();

        grid.CopyFrom(_grid, new GridCoordinate(-_grid.Width / 2, -_grid.Height / 2));

        var virus = new GridWalker(new GridPosition(GridCoordinate.Zero, GridDirection.Up));

        for (var burst = 0; burst < 10000000; burst++)
        {
            if (grid[virus.Coordinate] == State.Clean)
            {
                virus.TurnLeft();
                grid[virus.Coordinate] = State.Weakened;
            }
            else if (grid[virus.Coordinate] == State.Weakened)
            {
                answer++;
                grid[virus.Coordinate] = State.Infected;
            }
            else if (grid[virus.Coordinate] == State.Infected)
            {
                virus.TurnRight();
                grid[virus.Coordinate] = State.Flagged;
            }
            else
            {
                virus.Reverse();
                grid[virus.Coordinate] = State.Clean;
            }

            virus.Step();
        }

        return new PuzzleAnswer(answer, 2511957);
    }
}