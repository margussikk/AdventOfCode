using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2016.Day10;

[Puzzle(2016, 10, "Balance Bots")]
public class Day10PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Bot> _bots = [];
    private IReadOnlyList<GiveInstruction> _giveInstructions = [];

    public void ParseInput(string[] inputLines)
    {
        _bots = [.. inputLines.Where(line => line.StartsWith("bot")).Select(Bot.Parse)];
        _giveInstructions = [.. inputLines.Where(line => line.StartsWith("value")).Select(GiveInstruction.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var bots = _bots.Select(bot => bot.CleanCopy()).ToList();

        HandleChips(bots);

        var answer = bots.First(bot => bot.ComparedChips[0] == 17 && bot.ComparedChips[1] == 61).Id;

        return new PuzzleAnswer(answer, 157);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var bots = _bots.Select(b => b.CleanCopy()).ToList();

        var outputInstructions = HandleChips(bots);

        var answer = outputInstructions.Where(o => o.Target.Id <= 2)
                                       .Aggregate(1, (agg, curr) => agg * curr.ChipValue);

        return new PuzzleAnswer(answer, 1085);
    }

    private List<GiveInstruction> HandleChips(List<Bot> bots)
    {
        var outputInstructions = new List<GiveInstruction>();

        var botsDictionary = bots.ToDictionary(b => b.Id);

        var queue = new Queue<GiveInstruction>(_giveInstructions);
        while (queue.TryDequeue(out var instruction))
        {
            if (instruction.Target.Type == TargetType.Bot)
            {
                var bot = botsDictionary[instruction.Target.Id];

                var newInstructions = bot.Give(instruction.ChipValue);
                foreach (var newInstruction in newInstructions)
                {
                    queue.Enqueue(newInstruction);
                }
            }
            else
            {
                outputInstructions.Add(instruction);
            }
        }

        return outputInstructions;
    }
}