using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2017.Day18;

[Puzzle(2017, 18, "Duet")]
public class Day18PuzzleSolver : IPuzzleSolver
{
    private List<Instruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        _instructions = inputLines.Select(Instruction.Parse)
                                  .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var program = new Program(0, _instructions);

        program.Run(false);

        var answer = program.Outbox[^1];

        return new PuzzleAnswer(answer, 3423);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var program0 = new Program(0, _instructions);
        program0.Registers['p' - 'a'] = 0;

        var program1 = new Program(1, _instructions);
        program1.Registers['p' - 'a'] = 1;

        do
        {
            // Program 0
            program0.Run(true);
            foreach (var value in program0.Outbox)
            {
                program1.Inbox.Enqueue(value);
            }

            program0.Outbox.Clear();

            // Program 1
            program1.Run(true);
            foreach (var value in program1.Outbox)
            {
                program0.Inbox.Enqueue(value);
            }

            program1.Outbox.Clear();

        }
        while (program0.Inbox.Count > 0 || program1.Inbox.Count > 0);

        var answer = program1.SendCount;

        return new PuzzleAnswer(answer, 7493);
    }
}
