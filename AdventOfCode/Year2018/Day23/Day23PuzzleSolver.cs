using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2018.Day23;

[Puzzle(2018, 23, "Experimental Emergency Teleportation")]
public class Day23PuzzleSolver : IPuzzleSolver
{
    private List<Nanobot> _nanobots = [];

    public void ParseInput(string[] inputLines)
    {
        _nanobots = inputLines.Select(Nanobot.Parse).ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var nanobot = _nanobots.MaxBy(x => x.SignalRadius)!;

        var answer = _nanobots.Count(nb => nb.Coordinate.ManhattanDistanceTo(nanobot.Coordinate) <= nanobot.SignalRadius);

        return new PuzzleAnswer(answer, 889);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0L;

        var region = new Region3D(_nanobots.SelectMany(nb => nb.Corners()));
        var searchBox = new SearchBox(region);

        searchBox.Nanobots.AddRange(_nanobots);

        var queue = new PriorityQueue<SearchBox, SearchBoxRank>();
        queue.Enqueue(searchBox, searchBox.GetRank());

        while(queue.TryDequeue(out searchBox, out var rank))
        {
            if (searchBox.Volume == 1)
            {
                answer = rank.Distance;
                break;
            }

            foreach (var subSearchBox in searchBox.Divide())
            {
                foreach (var nanobot in searchBox.Nanobots)
                {
                    if (subSearchBox.InBounds(nanobot))
                    {
                        subSearchBox.Nanobots.Add(nanobot);
                    }
                }

                queue.Enqueue(subSearchBox, subSearchBox.GetRank());
            }
        }

        return new PuzzleAnswer(answer, 160646364);
    }
}