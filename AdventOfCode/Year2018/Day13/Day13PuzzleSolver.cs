using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;
using Microsoft.CodeAnalysis;

namespace AdventOfCode.Year2018.Day13;

[Puzzle(2018, 13, "Mine Cart Madness")]
public class Day13PuzzleSolver : IPuzzleSolver
{
    private Grid<GridDirection> _grid = new(0, 0);

    private readonly List<Cart> _carts = [];

    public void ParseInput(string[] inputLines)
    {
        _grid = inputLines.SelectToGrid((character, coordinate) =>
        {
            if (character is '<' or '>' or 'v' or '^')
            {
                _carts.Add(new Cart(coordinate, character.ParseArrowToGridDirection()));
            }

            return character switch
            {
                ' ' => GridDirection.None,
                '|' or 'v' or '^' => GridDirection.UpAndDown,
                '-' or '<' or '>' => GridDirection.LeftAndRight,
                '/' => GridDirection.UpAndLeft,
                '\\' => GridDirection.DownAndLeft,
                '+' => GridDirection.AllSides,
                _ => throw new NotImplementedException()
            };
        });
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var carts = _carts.Select(c => new Cart(c.Coordinate, c.Direction))
                          .OrderBy(c => c.Coordinate.Row)
                          .ThenBy(c => c.Coordinate.Column)
                          .ToList();

        while (true)
        {
            foreach (var cart in carts)
            {
                cart.Move(_grid);

                if (carts.Exists(c => c != cart && c.Coordinate == cart.Coordinate))
                {
                    var answer = $"{cart.Coordinate.Column},{cart.Coordinate.Row}";
                    return new PuzzleAnswer(answer, "123,18");
                }
            }

            carts = [.. carts.Where(c => !c.Crashed)
                             .OrderBy(c => c.Coordinate.Row)
                             .ThenBy(c => c.Coordinate.Column)];
        }
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var carts = _carts.Select(c => new Cart(c.Coordinate, c.Direction))
                          .OrderBy(c => c.Coordinate.Row)
                          .ThenBy(c => c.Coordinate.Column)
                          .ToList();

        while (carts.Count != 1)
        {
            foreach (var cart in carts.Where(c => !c.Crashed))
            {
                cart.Move(_grid);

                var otherCart = carts.Find(c => c != cart && c.Coordinate == cart.Coordinate);
                if (otherCart != null)
                {
                    cart.Crashed = true;
                    otherCart.Crashed = true;
                }
            }

            carts = [.. carts.Where(c => !c.Crashed)
                         .OrderBy(c => c.Coordinate.Row)
                         .ThenBy(c => c.Coordinate.Column)];

        }

        var answer = $"{carts[0].Coordinate.Column},{carts[0].Coordinate.Row}";
        return new PuzzleAnswer(answer, "71,123");
    }

    private void PrintGrid(List<Cart> carts)
    {
        for (var row = 0; row <= _grid.LastRow; row++)
        {
            for (var column = 0; column <= _grid.LastColumn; column++)
            {
                var coordinate = new GridCoordinate(row, column);
                char symbol = '0';

                var cart = carts.Find(c => c.Coordinate == coordinate);
                if (cart != null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    symbol = cart.Direction switch
                    {
                        GridDirection.Left => '<',
                        GridDirection.Right => '>',
                        GridDirection.Up => '^',
                        GridDirection.Down => 'v',
                        _ => throw new NotImplementedException()
                    };
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;

                    symbol = _grid[coordinate] switch
                    {
                        GridDirection.None => ' ',
                        GridDirection.UpAndDown => '|',
                        GridDirection.LeftAndRight => '-',
                        GridDirection.UpAndLeft => '/',
                        GridDirection.DownAndLeft => '\\',
                        GridDirection.AllSides => '+',
                        _ => throw new NotImplementedException()
                    };
                }

                Console.Write(symbol);
            }

            Console.WriteLine();
        }
    }
}