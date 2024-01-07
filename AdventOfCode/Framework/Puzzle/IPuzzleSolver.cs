namespace AdventOfCode.Framework.Puzzle;

public interface IPuzzleSolver
{
    void ParseInput(List<string> inputLines);

    PuzzleAnswer GetPartOneAnswer();

    PuzzleAnswer GetPartTwoAnswer();
}
