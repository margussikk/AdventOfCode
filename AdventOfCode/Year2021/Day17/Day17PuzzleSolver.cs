using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2021.Day17;

[Puzzle(2021, 17, "Trick Shot")]
public class Day17PuzzleSolver : IPuzzleSolver
{
    private int _targetXMin;
    private int _targetXMax;
    private int _targetYMax;
    private int _targetYMin;

    public void ParseInput(string[] inputLines)
    {
        var splits = inputLines[0]["target area: x=".Length..].Replace(", y=", "..").Split("..");

        _targetXMin = int.Parse(splits[0]);
        _targetXMax = int.Parse(splits[1]);
        _targetYMin = int.Parse(splits[2]);
        _targetYMax = int.Parse(splits[3]);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var lastYVelocity = Math.Abs(_targetYMin);
        var answer = lastYVelocity * (lastYVelocity - 1) / 2;

        return new PuzzleAnswer(answer, 5565);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0;
        var minXVelocity = (int)Math.Ceiling((-1 + Math.Sqrt(1 + 8 * _targetXMin)) / 2);

        for (var xVelocity = minXVelocity; xVelocity <= _targetXMax; xVelocity++)
        {
            for (var yVelocity = _targetYMin; yVelocity <= Math.Abs(_targetYMin) - 1; yVelocity++)
            {
                var hitTarget = HitsTarget(xVelocity, yVelocity);
                if (hitTarget)
                {
                    answer++;
                }
            }
        }

        return new PuzzleAnswer(answer, 2118);
    }

    private bool HitsTarget(int xVelocity, int yVelocity)
    {
        var x = 0;
        var y = 0;

        while (x <= _targetXMax && y >= _targetYMin && !(xVelocity == 0 && x < _targetXMin))
        {
            x += xVelocity;
            if (xVelocity > 0)
            {
                xVelocity--;
            }

            y += yVelocity;
            yVelocity--;

            if (x >= _targetXMin && x <= _targetXMax && y >= _targetYMin && y <= _targetYMax)
            {
                return true;
            }
        }

        return false;
    }
}