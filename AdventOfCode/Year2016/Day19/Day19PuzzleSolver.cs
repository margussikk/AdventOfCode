using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Mathematics;

namespace AdventOfCode.Year2016.Day19;

[Puzzle(2016, 19, "An Elephant Named Joseph")]
public class Day19PuzzleSolver : IPuzzleSolver
{
    private int _input;

    public void ParseInput(string[] inputLines)
    {
        _input = int.Parse(inputLines[0]);
    }

    // Josephus problem: https://www.youtube.com/watch?v=uCsD3ZGzMgE
    public PuzzleAnswer GetPartOneAnswer()
    {
        var smallerPowerOfTwo = MathFunctions.LargestPower(_input, 2);
        var answer = (_input - smallerPowerOfTwo) * 2 + 1;

        return new PuzzleAnswer(answer, 1815603);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var smallerPowerOfThree = MathFunctions.LargestPower(_input, 3);
        var largerPowerOfThree = smallerPowerOfThree * 3;

        var answer = _input - smallerPowerOfThree;

        var middle = (smallerPowerOfThree + largerPowerOfThree) / 2;
        if (_input > middle)
        {
            answer += _input - middle;
        }

        return new PuzzleAnswer(answer, 1410630);
    }
}