using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Year2019.IntCode;

namespace AdventOfCode.Year2019.Day19;

[Puzzle(2019, 19, "Tractor Beam")]
public class Day19PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<long> _program = [];

    public void ParseInput(string[] inputLines)
    {
        _program = inputLines[0].SelectToLongs(',');
    }

    public PuzzleAnswer GetPartOneAnswerOld()
    {
        var answer = 0L;

        for (var y = 0; y < 50; y++)
        {
            for (var x = 0; x < 50; x++)
            {
                if (IsInBeam(x, y))
                {
                    answer++;
                }
            }
        }

        return new PuzzleAnswer(answer, 183);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = 0L;

        int? leftX = null;
        int? rightX = null;

        for (var y = 0; y < 50; y++)
        {
            var x = 0;

            // Left edge
            if (leftX.HasValue)
            {
                x = leftX.Value;
            }

            while(true)
            {
                if (x >= 50)
                {
                    leftX = null;
                    rightX = null;
                    break;
                }
                else if (IsInBeam(x, y))
                {
                    leftX = x;
                    break;
                }
                else
                {
                    x++;
                }            
            }

            // Right edge
            rightX ??= leftX;
            if (rightX.HasValue)
            {
                x = rightX.Value;

                while (true)
                {
                    if (x >= 50)
                    {
                        rightX = 49;
                        break;
                    }
                    else if (!IsInBeam(x + 1, y))
                    {
                        rightX = x;
                        break;
                    }
                    else
                    {
                        x++;
                    }
                }
            }

            if (leftX.HasValue && rightX.HasValue)
            {
                answer += rightX.Value - leftX.Value + 1;
            }
        }

        return new PuzzleAnswer(answer, 183);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var beams = DetermineBeams();

        // Calculate approximate Y
        var leftRatio = 1D * beams.Left.Vector.DX / beams.Left.Vector.DY;
        var rightRatio = 1D * beams.Right.Vector.DX / beams.Right.Vector.DY;
        var y = Convert.ToInt32(Math.Ceiling(198 / ((rightRatio - leftRatio) * (2 - rightRatio))));
        var x = (int)beams.Left.GetX(y);

        // Find exact left beam X at row Y
        if (IsInBeam(x, y))
        {
            while(IsInBeam(x - 1, y))
            {
                x--;
            }
        }
        else
        {
            while (!IsInBeam(x, y))
            {
                x++;
            }
        }
        
        // Follow left beam
        while(!IsInBeam(x + 99, y - 99))
        {
            y++;

            while (!IsInBeam(x, y))
            {
                x++;
            }
        }

        var answer = x * 10_000 + (y - 99);

        return new PuzzleAnswer(answer, 11221248);
    }

    private bool IsInBeam(int x, int y)
    {
        var computer = new IntCodeComputer(_program);

        var result = computer.Run([x, y]);

        return result.Outputs[0] == 1L;
    }

    private Beams DetermineBeams()
    {
        var leftCoordinates = new List<Coordinate2D>();
        var rightCoordinates = new List<Coordinate2D>();
        var y = 9;

        // Left
        var leftX = 0;
        while (!IsInBeam(leftX, y))
        {
            leftX++;
        }

        // Right
        var rightX = leftX;
        if (IsInBeam(rightX + 1, y))
        {
            rightX++;
        }

        // New left
        var newY = y + 1;
        while (true)
        {
            var newLeftX = leftX;
            while (!IsInBeam(newLeftX, newY))
            {
                newLeftX++;
            }

            if (newLeftX == leftX)
            {
                var coordinate = new Coordinate2D(leftX, newY);
                leftCoordinates.Add(coordinate);
                if (leftCoordinates.Count == 2)
                {
                    break;
                }
            }

            leftX = newLeftX;
            newY++;
        }

        // New right
        newY = y + 1;
        while (true)
        {
            var newRightX = rightX;
            while (IsInBeam(newRightX + 1, newY))
            {
                newRightX++;
            }

            if (newRightX == rightX)
            {
                var coordinate = new Coordinate2D(rightX, newY);
                rightCoordinates.Add(coordinate);
                if (rightCoordinates.Count == 2)
                {
                    break;
                }
            }

            rightX = newRightX;
            newY++;
        }

        var leftBeam = new Beam()
        {
            Start = leftCoordinates[0],
            Vector = leftCoordinates[1] - leftCoordinates[0]
        };

        var rightBeam = new Beam()
        {
            Start = rightCoordinates[0],
            Vector = rightCoordinates[1] - rightCoordinates[0]
        };

        return new Beams(leftBeam, rightBeam);
    }

    private sealed record Beams(Beam Left, Beam Right);
}