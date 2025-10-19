using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Mathematics;

namespace AdventOfCode.Year2015.Day20;

[Puzzle(2015, 20, "Infinite Elves and Infinite Houses")]
public class Day20PuzzleSolver : IPuzzleSolver
{
    private long _input = 0;

    public void ParseInput(string[] inputLines)
    {
        _input = long.Parse(inputLines[0]);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = Enumerable.Range(1, 1000000)
                               .First(house => MathFunctions.SumOfDivisors(house) * 10 >= _input);

        return new PuzzleAnswer(answer, 665280);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0;

        for (var house = 1; house < int.MaxValue; house++)
        {
            var presents = 0L;

            var divisors = MathFunctions.Divisors(house);
            foreach (var divisor in divisors)
            {
                // Each elf delivers to only 50 houses and each house gets 11 presents from each elf.
                // Elf 1 delivers to houses 1, 2, 3, ..., 50; Elf 2 delivers to houses 2, 4, 6, ..., 100; etc.
                if (house / divisor <= 50)
                {
                    presents += divisor * 11;
                }
            }

            if (presents >= _input)
            {
                answer = house;
                break;
            }
        }

        return new PuzzleAnswer(answer, 705600);
    }
}