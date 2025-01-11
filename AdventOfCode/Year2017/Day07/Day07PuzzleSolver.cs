using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2017.Day07;

[Puzzle(2017, 7, "Recursive Circus")]
public class Day07PuzzleSolver : IPuzzleSolver
{
    private List<Program> _programs = [];

    public void ParseInput(string[] inputLines)
    {
        var programs = new Dictionary<string, Program>();

        foreach (var line in inputLines)
        {
            var splits = line.Split("->", StringSplitOptions.TrimEntries);

            // Name and weight
            var nameAndWeightSplits = splits[0].Split(' ', StringSplitOptions.TrimEntries);
            if (!programs.TryGetValue(nameAndWeightSplits[0], out var program))
            {
                program = new Program(nameAndWeightSplits[0]);
                programs[program.Name] = program;
            }

            program.Weight = int.Parse(nameAndWeightSplits[1][1..^1]);

            if (splits.Length > 1)
            {
                var aboveProgramNames = splits[1].Split(',', StringSplitOptions.TrimEntries);
                foreach(var aboveProgramName in aboveProgramNames)
                {
                    if (!programs.TryGetValue(aboveProgramName, out var aboveProgram))
                    {
                        aboveProgram = new Program(aboveProgramName);
                        programs[aboveProgramName] = aboveProgram;
                    }

                    program.AbovePrograms.Add(aboveProgram);
                    aboveProgram.BelowProgram = program;
                }
            }
        }

        _programs = [.. programs.Values];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var bottomProgram = _programs.First(p => p.BelowProgram == null);

        return new PuzzleAnswer(bottomProgram.Name, "wiapj");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var bottomProgram = _programs.First(p => p.BelowProgram == null);

        var (found, answer) = bottomProgram.FindIncorrectWeight();
        if (!found)
        {
            throw new InvalidOperationException("Program with incorrect weight not found");
        }

        return new PuzzleAnswer(answer, 1072);
    }
}