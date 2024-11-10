using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Mathematics;

namespace AdventOfCode.Year2019.Day12;

[Puzzle(2019, 12, "The N-Body Problem")]
public class Day12PuzzleSolver : IPuzzleSolver
{
    private List<Moon> _moons = [];

    public void ParseInput(string[] inputLines)
    {
        _moons = inputLines.Select(Moon.Parse)
                           .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var moons = _moons.Select(m => m.Clone())
                          .ToList();

        for (var step = 0; step < 1000; step++)
        {
            SimulateMoons(moons);
        }

        var answer = moons.Sum(m => m.GetTotalEnergy());

        return new PuzzleAnswer(answer, 14780);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var moons = _moons.Select(m => m.Clone())
                          .ToList();

        var xStates = new Dictionary<(int x1, int vx1, int x2, int vx2, int x3, int vx3, int x4, int vx4), int>();
        var xCycleLength = 0;

        var yStates = new Dictionary<(int y1, int vy1, int y2, int vy2, int y3, int vy3, int y4, int vy4), int>();
        var yCycleLength = 0;

        var zStates = new Dictionary<(int z1, int vz1, int z2, int vz2, int z3, int vz3, int z4, int vz4), int>();
        var zCycleLength = 0;

        var step = 0;
        while(xCycleLength == 0 || yCycleLength == 0 || zCycleLength == 0)
        {
            SimulateMoons(moons);

            // X
            if (xCycleLength == 0)
            {
                var xState = (moons[0].X, moons[0].VX, moons[1].X, moons[1].VX, moons[2].X, moons[2].VX, moons[3].X, moons[3].VX);
                if (xStates.TryGetValue(xState, out var xStart))
                {
                    xCycleLength = step - xStart;
                }
                else
                {
                    xStates.Add(xState, step);
                }
            }


            // Y
            if (yCycleLength == 0)
            {
                var yState = (moons[0].Y, moons[0].VY, moons[1].Y, moons[1].VY, moons[2].Y, moons[2].VY, moons[3].Y, moons[3].VY);
                if (yStates.TryGetValue(yState, out var yStart))
                {
                    yCycleLength = step - yStart;
                }
                else
                {
                    yStates.Add(yState, step);
                }
            }

            // Z
            if (zCycleLength == 0)
            {
                var zState = (moons[0].Z, moons[0].VZ, moons[1].Z, moons[1].VZ, moons[2].Z, moons[2].VZ, moons[3].Z, moons[3].VZ);
                if (zStates.TryGetValue(zState, out var zStart))
                {
                    zCycleLength = step - zStart;
                }
                else
                {
                    zStates.Add(zState, step);
                }
            }

            step++;
        }

        var answer = new long[] { xCycleLength, yCycleLength, zCycleLength }.LeastCommonMultiple();

        return new PuzzleAnswer(answer, 279751820342592L);
    }

    private static void SimulateMoons(List<Moon> moons)
    {
        // Apply gravity
        for (var index1 = 0; index1 < moons.Count - 1; index1++)
        {
            for (var index2 = index1 + 1; index2 < moons.Count; index2++)
            {
                moons[index1].ApplyGravity(moons[index2]);
                moons[index2].ApplyGravity(moons[index1]);
            }
        }

        // Apply velocity
        foreach (var moon in moons)
        {
            moon.X += moon.VX;
            moon.Y += moon.VY;
            moon.Z += moon.VZ;
        }
    }
}