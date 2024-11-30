using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;
using System.Numerics;

namespace AdventOfCode.Year2021.Day19;

[Puzzle(2021, 19, "Beacon Scanner")]
public class Day19PuzzleSolver : IPuzzleSolver
{
    private readonly List<Scanner> _scanners = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        for (var scannerIndex = 0; scannerIndex < chunks.Count; scannerIndex++)
        {
            var chunk = chunks[scannerIndex];
            if (!chunk[0].StartsWith("--- scanner"))
            {
                throw new InvalidOperationException("Failed to parse input");
            }

            var scanner = new Scanner(scannerIndex + 1);
            foreach (var beaconMeasurementLine in chunk.Skip(1))
            {
                var measurement = Coordinate3D.Parse(beaconMeasurementLine);
                scanner.AddBeaconMeasurement(measurement);
            }

            _scanners.Add(scanner);
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var alignedScanners = GetAlignedScanners();

        var answer = alignedScanners
            .SelectMany(s => s.BeaconLocations[s.Orientation]
                .Select(b => new Coordinate3D(s.AbsoluteLocation.X + b.X,
                                              s.AbsoluteLocation.Y + b.Y,
                                              s.AbsoluteLocation.Z + b.Z)))
            .Distinct()
            .Count();

        return new PuzzleAnswer(answer, 306);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var alignedScanners = GetAlignedScanners();

        var answer = long.MinValue;
        for (var i = 0; i < alignedScanners.Count - 1; i++)
        {
            for (var j = i + 1; j < alignedScanners.Count; j++)
            {
                answer = Math.Max(answer, MeasurementFunctions.ManhattanDistance(alignedScanners[i].AbsoluteLocation, alignedScanners[j].AbsoluteLocation));
            }
        }

        return new PuzzleAnswer(answer, 9764);
    }

    private List<Scanner> GetAlignedScanners()
    {
        var scanners = _scanners.Select(s => s.Clone())
                                .ToList();

        scanners[0].Aligned = true;

        var processedScanners = 0UL;
        while (BitOperations.PopCount(processedScanners) != scanners.Count)
        {
            foreach (var alignedScanner in scanners.Where(s => s.Aligned))
            {
                var scannerBitmask = 1UL << alignedScanner.Id;
                if ((processedScanners & scannerBitmask) == scannerBitmask)
                {
                    continue;
                }

                foreach (var misalignedScanner in scanners.Where(s => !s.Aligned))
                {
                    TryAlignScanner(alignedScanner, misalignedScanner);
                }

                processedScanners |= scannerBitmask;
            }
        }

        return scanners;
    }

    private static void TryAlignScanner(Scanner alignedScanner, Scanner misalignedScanner)
    {
        for (var orientation = 0; orientation < 24; orientation++)
        {
            var differences = new List<Vector3D>();
            foreach (var alignedScannerBeaconLocation in alignedScanner.BeaconLocations[alignedScanner.Orientation])
            {
                foreach (var misalignedScannerBeaconLocation in misalignedScanner.BeaconLocations[orientation])
                {
                    var difference = alignedScannerBeaconLocation - misalignedScannerBeaconLocation;
                    differences.Add(difference);
                }
            }

            var twelveDifferences = differences.ToLookup(x => x)
                                               .FirstOrDefault(x => x.Count() >= 12);
            if (twelveDifferences == null) continue;

            misalignedScanner.Aligned = true;
            misalignedScanner.Orientation = orientation;
            misalignedScanner.AbsoluteLocation = alignedScanner.AbsoluteLocation + twelveDifferences.Key;

            return;
        }
    }
}