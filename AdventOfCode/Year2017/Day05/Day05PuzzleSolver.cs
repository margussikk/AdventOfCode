using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2017.Day05;

[Puzzle(2017, 5, "A Maze of Twisty Trampolines, All Alike")]
public class Day05PuzzleSolver : IPuzzleSolver
{
    private List<int> _jumpOffsets = [];

    public void ParseInput(string[] inputLines)
    {
        _jumpOffsets = inputLines.Select(int.Parse).ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(false);

        return new PuzzleAnswer(answer, 356945);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(true);

        return new PuzzleAnswer(answer, 28372145);
    }

    private int GetAnswer(bool decreaseJumpOffset)
    {
        var answer = 0;

        var jumpOffsets = _jumpOffsets.ToList();

        var offset = 0;
        while (offset >= 0 && offset < jumpOffsets.Count)
        {
            var jump = jumpOffsets[offset];

            if (decreaseJumpOffset && jump >= 3)
            {
                jumpOffsets[offset]--;
            }
            else
            {
                jumpOffsets[offset]++;
            }

            offset += jump;

            answer++;
        }

        return answer;
    }
}