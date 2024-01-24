using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2021.Day22;

[Puzzle(2021, 22, "Reactor Reboot")]
public class Day22PuzzleSolver : IPuzzleSolver
{
    private readonly List<Cuboid> _cuboids = [];

    public void ParseInput(string[] inputLines)
    {
        foreach (var line in inputLines)
        {
            var splits = line.Split(" ");
            var on = splits[0] == "on";

            splits = splits[1]
                .Replace("x=", "..")
                .Replace(",y=", "..")
                .Replace(",z=", "..")
                .Replace("..", " ")
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var cuboid = new Cuboid(
                int.Parse(splits[0]),
                int.Parse(splits[1]),
                int.Parse(splits[2]),
                int.Parse(splits[3]),
                int.Parse(splits[4]),
                int.Parse(splits[5]),
                on);

            _cuboids.Add(cuboid);
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswerBySplittingCuboids(_cuboids.Where(c => c.X1 >= -50 && c.X2 <= 50 && c.Y1 >= -50 && c.Y2 <= 50 && c.Z1 >= -50 && c.Z2 <= 50));

        return new PuzzleAnswer(answer, 580810);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswerBySplittingCuboids(_cuboids);

        return new PuzzleAnswer(answer, 1265621119006734L);
    }


    // Initial, but slow solution, kept for history. Uses set theory where A + B = A + B - intersection(A, B).
    // It counts intersection cuboids for "negative count".
    private static long GetAnswerByUsingNegativeCuboids(IEnumerable<Cuboid> inputCuboids)
    {
        var cuboids = new List<Cuboid>();

        foreach (var inputCuboid in inputCuboids)
        {
            var addedCuboids = new List<Cuboid>();

            foreach (var cuboid in cuboids)
            {
                if (cuboid.Intersects(inputCuboid))
                {
                    var intersection = cuboid.Intersection(inputCuboid);
                    addedCuboids.Add(intersection);
                }
            }

            if (inputCuboid.On)
            {
                addedCuboids.Add(inputCuboid);
            }

            cuboids.AddRange(addedCuboids);
        }

        return cuboids.Sum(x => x.Count);
    }

    // Faster solution. Splits cuboids into other cuboids if cuboids intersect
    private static long GetAnswerBySplittingCuboids(IEnumerable<Cuboid> inputCuboids)
    {
        var cuboids = new List<Cuboid>();

        foreach (var inputCuboid in inputCuboids)
        {
            var currentCuboids = new List<Cuboid>();

            foreach (var cuboid in cuboids)
            {
                if (cuboid.Intersects(inputCuboid))
                {
                    var splitCuboids = cuboid.Split(inputCuboid);
                    currentCuboids.AddRange(splitCuboids);
                }
                else
                {
                    currentCuboids.Add(cuboid);
                }
            }

            if (inputCuboid.On)
            {
                currentCuboids.Add(inputCuboid);
            }

            cuboids = currentCuboids;
        }

        return cuboids.Sum(c => c.Count);
    }
}