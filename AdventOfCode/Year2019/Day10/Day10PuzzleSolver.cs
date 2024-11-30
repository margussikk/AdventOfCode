using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2019.Day10;

[Puzzle(2019, 10, "Monitoring Station")]
public class Day10PuzzleSolver : IPuzzleSolver
{
    private readonly List<Coordinate2D> _asteroidCoordinates = [];

    public void ParseInput(string[] inputLines)
    {
        for (var y = 0; y < inputLines.Length; y++)
        {
            for (var x = 0; x < inputLines[y].Length; x++)
            {
                if (inputLines[y][x] == '#')
                {
                    _asteroidCoordinates.Add(new Coordinate2D(x, y));
                }
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var (_, answer) = GetMonitoringStationDetails();

        return new PuzzleAnswer(answer, 274);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0L;

        var (laserCoordinate, _) = GetMonitoringStationDetails();

        // Get vaporization info for all asteroids: coordinate, degree and distance from monitoring station
        var vaporizationInfoList = new List<AsteroidVaporizationDetails>();

        foreach (var asteroidCoordinate in _asteroidCoordinates.Where(a => a != laserCoordinate))
        {
            var vector = asteroidCoordinate - laserCoordinate;

            var degree = Math.Atan2(vector.DX, -vector.DY); // NB! Y and X are reversed
            if (degree < 0)
            {
                degree += 2 * Math.PI;
            }

            var distance = Math.Abs(vector.DX) + Math.Abs(vector.DY);

            var details = new AsteroidVaporizationDetails(asteroidCoordinate, degree, distance);
            vaporizationInfoList.Add(details);
        }

        // Vaporize asteroids around the clock
        var queue = new Queue<AsteroidVaporizationDetails>(
            vaporizationInfoList.OrderBy(p => p.Degree)
                                .ThenBy(pos => pos.Distance));

        double? laserDegree = null;

        var counter = 0;
        while (queue.TryDequeue(out var vaporizationInfo))
        {
            if (laserDegree.HasValue && Math.Abs(vaporizationInfo.Degree - laserDegree.Value) < double.Epsilon) // Deal with it on the next round
            {
                queue.Enqueue(vaporizationInfo);
            }
            else
            {
                laserDegree = vaporizationInfo.Degree;

                counter++;
                if (counter != 200) continue;

                answer = vaporizationInfo.Coordinate.X * 100 + vaporizationInfo.Coordinate.Y;
                break;
            }
        }

        return new PuzzleAnswer(answer, 305);
    }

    private (Coordinate2D Coordinate, int Detected) GetMonitoringStationDetails()
    {
        Coordinate2D coordinate = new(0, 0);
        var detected = int.MinValue;

        foreach (var currentAsteroidCoordinate in _asteroidCoordinates)
        {
            var sightVectors = new HashSet<Vector2D>();

            foreach (var otherAsteroidCoordinate in _asteroidCoordinates.Where(c => c != currentAsteroidCoordinate))
            {
                var vector = otherAsteroidCoordinate - currentAsteroidCoordinate;
                sightVectors.Add(vector.Normalize());
            }

            if (sightVectors.Count <= detected) continue;

            detected = sightVectors.Count;
            coordinate = currentAsteroidCoordinate;
        }

        return (coordinate, detected);
    }

    private sealed record AsteroidVaporizationDetails(Coordinate2D Coordinate, double Degree, long Distance);
}