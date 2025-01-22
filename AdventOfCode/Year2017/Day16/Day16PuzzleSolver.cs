using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2017.Day16;

[Puzzle(2017, 16, "Permutation Promenade")]
public class Day16PuzzleSolver : IPuzzleSolver
{
    private readonly List<IDanceMove> _danceMoves = [];

    public void ParseInput(string[] inputLines)
    {
        var splits = inputLines[0].Split(',');

        foreach (var danceMoveInput in splits)
        {
            if (danceMoveInput.Contains('/'))
            {
                if (danceMoveInput[0] == 'x')
                {
                    _danceMoves.Add(ExchangeDanceMove.Parse(danceMoveInput));
                }
                else if (danceMoveInput[0] == 'p')
                {
                    _danceMoves.Add(PartnerDanceMove.Parse(danceMoveInput));
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                _danceMoves.Add(SpinDanceMove.Parse(danceMoveInput));
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var programs = Enumerable.Range(0, 'p' - 'a' + 1)
            .Select(x => Convert.ToChar(x + 'a'))
            .ToList();

        foreach (var danceMove in _danceMoves)
        {
            danceMove.DoMove(programs);
        }

        var answer = new string([.. programs]);

        return new PuzzleAnswer(answer, "dcmlhejnifpokgba");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        const int totalDances = 1_000_000_000;
        var answer = string.Empty;

        var programs = Enumerable.Range(0, 'p' - 'a' + 1)
            .Select(x => Convert.ToChar(x + 'a'))
            .ToList();

        var danceStarts = new Dictionary<string, int>();

        for (var dance = 0; dance < totalDances; dance++)
        {
            var key = new string([.. programs]);
            if (danceStarts.TryGetValue(key, out var danceStart))
            {
                var cycleLength = dance - danceStart;
                var reminder = (totalDances - dance) % cycleLength;

                answer = danceStarts.First(x => x.Value == reminder).Key;
                break;
            }

            danceStarts[key] = dance;

            foreach (var danceMove in _danceMoves)
            {
                danceMove.DoMove(programs);
            }
        }

        return new PuzzleAnswer(answer, "ifocbejpdnklamhg");
    }
}