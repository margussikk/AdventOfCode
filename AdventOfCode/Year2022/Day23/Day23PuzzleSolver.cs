using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2022.Day23;

[Puzzle(2022, 23, "Unstable Diffusion")]
public class Day23PuzzleSolver : IPuzzleSolver
{
    private readonly List<Elf> _elves = [];

    public void ParseInput(string[] inputLines)
    {
        for (var row = 0; row < inputLines.Length; row++)
        {
            for (var column = 0; column < inputLines[row].Length; column++)
            {
                if (inputLines[row][column] == '#')
                {
                    _elves.Add(new Elf(new GridCoordinate(row, column)));
                }
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var (_, elves) = SpreadElves(10);

        var minRow = elves[0].Location.Row;
        var maxRow = elves[0].Location.Row;
        var minColumn = elves[0].Location.Column;
        var maxColumn = elves[0].Location.Column;

        foreach (var location in elves.Select(e => e.Location))
        {
            minRow = int.Min(minRow, location.Row);
            maxRow = int.Max(maxRow, location.Row);

            minColumn = int.Min(minColumn, location.Column);
            maxColumn = int.Max(maxColumn, location.Column);
        }

        var answer = (maxRow - minRow + 1) * (maxColumn - minColumn + 1) - elves.Count;

        return new PuzzleAnswer(answer, 3970);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var (answer, _) = SpreadElves(int.MaxValue);

        return new PuzzleAnswer(answer, 923);
    }

    private (int rounds, List<Elf> elves) SpreadElves(int maxRounds)
    {
        // Estimate how large grid could grow and adjust locations
        var maxRow = 0;
        var maxColumn = 0;

        foreach (var location in _elves.Select(e => e.Location))
        {
            maxRow = int.Max(maxRow, location.Row);
            maxColumn = int.Max(maxColumn, location.Column);
        }

        // Use a grid that's magic times larger than input. Using InfiniteBitGrid was about 40% slower.
        var magicMultiplier = 4;
        var locationGrid = new BitGrid(maxRow * magicMultiplier, maxColumn * magicMultiplier);

        var rowOffset = maxRow * (magicMultiplier - 1) / 2;
        var columnOffset = maxColumn * (magicMultiplier - 1) / 2;

        var elves = new List<Elf>(_elves.Select(e => new Elf(e.Location.Move(GridDirection.Right, rowOffset).Move(GridDirection.Down, columnOffset))));
        foreach (var location in elves.Select(e => e.Location))
        {
            locationGrid[location] = true;
        }

        // Move elves
        var round = 0;
        for (; round < maxRounds; round++)
        {
            var someoneMoved = false;
            var proposedLocations = new Dictionary<GridCoordinate, List<Elf>>();

            // Propose a move
            var proposalIndex = round % 4;
            foreach (var elf in elves)
            {
                var madeProposal = false;

                if (elf.Location.AroundNeighbors().Any(coordinate => locationGrid[coordinate]))
                {
                    for (var i = 0; i < 4; i++)
                    {
                        var personalProposalIndex = (proposalIndex + i) % 4;

                        var neighbors = personalProposalIndex switch
                        {
                            0 => elf.Location.UpNeighbors(), // NW, N, NE
                            1 => elf.Location.DownNeighbors(), // SW, S, SE
                            2 => elf.Location.LeftNeighbors(), // NW, W, SW
                            3 => elf.Location.RightNeighbors(), // NE, E, SE
                            _ => throw new InvalidOperationException()
                        };

                        if (!neighbors.Any(coordinate => locationGrid[coordinate]))
                        {
                            var proposedLocation = personalProposalIndex switch
                            {
                                0 => elf.Location.Move(GridDirection.Up), // N
                                1 => elf.Location.Move(GridDirection.Down), // S
                                2 => elf.Location.Move(GridDirection.Left), // W
                                3 => elf.Location.Move(GridDirection.Right), // E
                                _ => throw new InvalidOperationException()
                            };

                            // Add elf to location proposers list
                            if (!proposedLocations.TryGetValue(proposedLocation, out var locationProposers))
                            {
                                locationProposers = [];
                                proposedLocations[proposedLocation] = locationProposers;
                            }

                            locationProposers.Add(elf);

                            madeProposal = true;
                            break;
                        }
                    }
                }

                someoneMoved = someoneMoved || madeProposal;
            }

            // Make the moves
            foreach (var kpv in proposedLocations)
            {
                if (kpv.Value.Count == 1)
                {
                    var elf = kpv.Value[0];

                    // Clear previous location
                    locationGrid[elf.Location] = false;

                    // Set new location
                    elf.Location = kpv.Key;
                    locationGrid[elf.Location] = true;
                }
            }

            if (!someoneMoved)
            {
                break;
            }
        }

        return (round + 1, elves);
    }
}