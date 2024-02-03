using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2023.Day24;

[Puzzle(2023, 24, "Never Tell Me The Odds")]
public class Day24PuzzleSolver : IPuzzleSolver
{
    private List<Hailstone> _hailstones = [];

    public void ParseInput(string[] inputLines)
    {
        _hailstones = inputLines.Select(Hailstone.Parse)
                                .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var minCoordinateValue = 200_000_000_000_000L;
        var maxCoordinateValue = 400_000_000_000_000L;

        var answer = 0;

        for (var i = 0; i < _hailstones.Count - 1; i++)
        {
            for (var j = i + 1; j < _hailstones.Count; j++)
            {
                var hailstone1 = _hailstones[i];
                var hailstone2 = _hailstones[j];

                var coordinate1 = new Coordinate2D(hailstone1.X, hailstone1.Y);
                var vector1 = new Vector2D(hailstone1.DX, hailstone1.DY);
                var line1 = new Line2D(coordinate1, vector1);

                var coordinate2 = new Coordinate2D(hailstone2.X, hailstone2.Y);
                var vector2 = new Vector2D(hailstone2.DX, hailstone2.DY);
                var line2 = new Line2D(coordinate2, vector2);

                if (line1.TryFindIntersectionCoordinate(line2, out var coordinate) &&
                    coordinate.X >= minCoordinateValue && coordinate.X <= maxCoordinateValue &&
                    coordinate.Y >= minCoordinateValue && coordinate.Y <= maxCoordinateValue &&
                    HappensInTheFuture(hailstone1, coordinate) &&
                    HappensInTheFuture(hailstone2, coordinate))
                {
                    answer++;
                }
            }
        }

        return new PuzzleAnswer(answer, 20434);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        // Rock hits hailstone when all following three conditions are true:
        // rx + t * rdx = hx + t * hdx
        // ry + t * rdy = hy + t * hdy
        // rz + t * rdz = hz + t * hdz

        // Solving for time t:
        //      hx - rx     hy - ry     hz - rz
        // t = --------- = --------- = ---------
        //     rdx - hdx   rdy - hdy   rdz - hdz
        //
        // When solving for XY plane then the first two equations must be equal:
        // (hx - rx) / (rdx - hdx) = (hy - ry) / (rdy - hdy) =>
        // (hx - rx) * (rdy - hdy) = (hy - ry) * (rdx - hdx) =>
        // hx * rdy - hx * hdy - rx * rdy + rx * hdy = hy * rdx - hy * hdx - ry * rdx + ry * hdx =>
        // ry * rdx - rx * rdy = hy * rdx - hy * hdx + ry * hdx - hx * rdy + hx * hdy - rx * hdy
        //
        // Use the above equation with two hailstones
        // ry * rdx - rx * rdy = h1y * rdx - h1y * h1dx + ry * h1dx - h1x * rdy + h1x * h1dy - rx * h1dy
        // ry * rdx - rx * rdy = h2y * rdx - h2y * h2dx + ry * h2dx - h2x * rdy + h2x * h2dy - rx * h2dy
        //
        // Left sides are equal, so equate right sides:
        // h1y * rdx - h1y * h1dx + ry * h1dx - h1x * rdy + h1x * h1dy - rx * h1dy = h2y * rdx - h2y * h2dx + ry * h2dx - h2x * rdy + h2x * h2dy - rx * h2dy
        //
        // Move all the unknown values to the left side hand and known values to right side of the equation:
        // (h2dy - h1dy) * rx + (h1dx - h2dx) * ry + (h1y - h2y) * rdx + (h2x - h1x) * rdy = h2x * h2dy - h2y * h2dx - h1x * h1dy + h1y * h1dx
        //
        // Doing the same process for XZ and YZ planes gives us additionally:
        // (h2dz - h1dz) * rx + (h1dx - h2dx) * rz + (h1z - h2z) * rdx + (h2x - h1x) * rdz = h1z * h1dz + h1x * h1dz - h2z * h2dx + h2x * h2dz
        // (h2dz - h1dz) * ry + (h1dy - h2dy) * rz + (h1z - h2z) * rdy + (h2y - h1y) * rdz = h1z * h1dz + h1y * h1dz - h2z * h2dy + h2y * h2dz

        // Solve the equation with 6 unknowns using 3 hailstones and Gaussian elimination.

        var matrix = new Matrix<double>(6, 7);
        matrix.SetRow(0, BuildXYCoefficients(_hailstones[0], _hailstones[1]));
        matrix.SetRow(1, BuildXYCoefficients(_hailstones[0], _hailstones[2]));
        matrix.SetRow(2, BuildXZCoefficients(_hailstones[0], _hailstones[1]));
        matrix.SetRow(3, BuildXZCoefficients(_hailstones[0], _hailstones[2]));
        matrix.SetRow(4, BuildYZCoefficients(_hailstones[0], _hailstones[1]));
        matrix.SetRow(5, BuildYZCoefficients(_hailstones[0], _hailstones[2]));

        if (!LinearEquationSolver.TrySolveLinearEquation(matrix, out var doubleValues))
        {
            throw new InvalidOperationException("Couldn't solve the equation");
        }

        var longValues = doubleValues.Select(Convert.ToInt64).ToList();

        var rockHailstone = new Hailstone(longValues[0], longValues[1], longValues[2], longValues[3], longValues[4], longValues[5]);

        var answer = rockHailstone.X + rockHailstone.Y + rockHailstone.Z;

        return new PuzzleAnswer(answer, 1025127405449117L);
    }


    private static bool HappensInTheFuture(Hailstone hailstone, Coordinate2D coordinate)
    {
        // time = distance / velocity
        // Is time positive?
        return (coordinate.X - hailstone.X) / hailstone.DX >= 0;
    }

    private static double[] BuildXYCoefficients(Hailstone hailstone1, Hailstone hailstone2)
    {
        // (hx - rx) / (rdx - hdx) = (hy - ry) / (rdy - hdy)
        // (hx - rx) * (rdy - hdy) = (hy - ry) * (rdx - hdx)
        // hx * rdy - hx * hdy - rx * rdy + rx * hdy = hy * rdx - hy * hdx - ry * rdx + ry * hdx =>
        // ry * rdx - rx * rdy = hy * rdx - hy * hdx + ry * hdx - hx * rdy + hx * hdy - rx * hdy
        // h1y * rdx - h1y * h1dx + ry * h1dx - h1x * rdy + h1x * h1dy - rx * h1dy = h2y * rdx - h2y * h2dx + ry * h2dx - h2x * rdy + h2x * h2dy - rx * h2dy
        // (h2dy - h1dy) * rx + (h1dx - h2dx) * ry + (h1y - h2y) * rdx + (h2x - h1x) * rdy = h2x * h2dy - h2y * h2dx - h1x * h1dy + h1y * h1dx

        var coefficients = new double[]
        {
            // Left side hand
            hailstone2.DY - hailstone1.DY, // X
            hailstone1.DX - hailstone2.DX, // Y
            0, // Z
            hailstone1.Y - hailstone2.Y, // DX
            hailstone2.X - hailstone1.X, // DY
            0, // DZ

            // Right side hand
            hailstone2.X * hailstone2.DY - hailstone2.Y * hailstone2.DX -
            hailstone1.X * hailstone1.DY + hailstone1.Y * hailstone1.DX
        };

        return coefficients;
    }

    private static double[] BuildXZCoefficients(Hailstone hailstone1, Hailstone hailstone2)
    {
        // (hx - rx) / (rdx - hdx) = (hz - rz) / (rdz - hdz)
        // (hx - rx) * (rdz - hdz) = (hz - rz) * (rdx - hdx)
        // hx * rdz - hx * hdz - rx * rdz + rx * hdz = hz * rdx - hz * hdx - rz * rdx + rz * hdx =>
        // rz * rdx - rx * rdz = hz * rdx - hz * hdx + rz * hdx - hx * rdz + hx * hdz - rx * hdz
        // h1z * rdx - h1z * h1dx + rz * h1dx - h1x * rdz + h1x * h1dz - rx * h1dz = h2z * rdx - h2z * h2dx + rz * h2dx - h2x * rdz + h2x * h2dz - rx * h2dz
        // (h2dz - h1dz) * rx + (h1dx - h2dx) * rz + (h1z - h2z) * rdx + (h2x - h1x) * rdz = h1z * h1dz + h1x * h1dz - h2z * h2dx + h2x * h2dz

        var coefficients = new double[]
        {
            // Left side hand
            hailstone2.DZ - hailstone1.DZ, // X
            0, // Y
            hailstone1.DX - hailstone2.DX, // Z
            hailstone1.Z - hailstone2.Z, // DX
            0, // DY
            hailstone2.X - hailstone1.X, // DZ

            // Right side hand
            hailstone2.X * hailstone2.DZ - hailstone2.Z * hailstone2.DX -
            hailstone1.X * hailstone1.DZ + hailstone1.Z * hailstone1.DX
        };

        return coefficients;
    }

    private static double[] BuildYZCoefficients(Hailstone hailstone1, Hailstone hailstone2)
    {
        // (hy - ry) / (rdy - hdy) = (hz - rz) / (rdz - hdz)
        // (hy - ry) * (rdz - hdz) = (hz - rz) * (rdy - hdy)
        // hy * rdz - hy * hdz - ry * rdz + ry * hdz = hz * rdy - hz * hdy - rz * rdy + rz * hdy =>
        // rz * rdy - ry * rdz = hz * rdy - hz * hdy + rz * hdy - hy * rdz + hy * hdz - ry * hdz
        // h1z * rdy - h1z * h1dy + rz * h1dy - h1y * rdz + h1y * h1dz - ry * h1dz = h2z * rdy - h2z * h2dy + rz * h2dy - h2y * rdz + h2y * h2dz - ry * h2dz
        // (h2dz - h1dz) * ry + (h1dy - h2dy) * rz + (h1z - h2z) * rdy + (h2y - h1y) * rdz = h1z * h1dz + h1y * h1dz - h2z * h2dy + h2y * h2dz

        var coefficients = new double[]
        {
            // Left side hand
            0, // X
            hailstone2.DZ - hailstone1.DZ, // Y
            hailstone1.DY - hailstone2.DY, // Z
            0, // DX
            hailstone1.Z - hailstone2.Z, // DY
            hailstone2.Y - hailstone1.Y, // DZ

            // Right side hand
            hailstone2.Y * hailstone2.DZ - hailstone2.Z * hailstone2.DY -
            hailstone1.Y * hailstone1.DZ + hailstone1.Z * hailstone1.DY
        };

        return coefficients;
    }
}
