using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2016.Day04;

[Puzzle(2016, 4, "Security Through Obscurity")]
public class Day04PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Room> _rooms = [];

    public void ParseInput(string[] inputLines)
    {
        _rooms = [.. inputLines.Select(Room.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _rooms.Where(room => room.IsReal())
                           .Sum(room => room.SectorId);

        return new PuzzleAnswer(answer, 173787);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        foreach (var room in _rooms.Where(room => room.IsReal()))
        {
            var decryptedName = room.GetDecryptedName();
            if (decryptedName == "northpole object storage")
            {
                return new PuzzleAnswer(room.SectorId, 548);
            }
        }

        return new PuzzleAnswer("TODO", "TODO");
    }
}