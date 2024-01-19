namespace AdventOfCode.Framework.Puzzle;

public interface IPuzzleSolver
{
    void ParseInput(string[] inputLines);

    PuzzleAnswer GetPartOneAnswer();

    PuzzleAnswer GetPartTwoAnswer();
}
