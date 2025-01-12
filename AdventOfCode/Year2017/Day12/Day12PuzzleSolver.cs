using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2017.Day12;

[Puzzle(2017, 12, "Digital Plumber")]
public class Day12PuzzleSolver : IPuzzleSolver
{
    private List<Pipe> _pipes = [];

    public void ParseInput(string[] inputLines)
    {
        _pipes = inputLines.Select(Pipe.Parse)
                           .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var groups = GetGroups();

        var groupContainingProgram0 = groups.First(g => g.Contains(0));

        return new PuzzleAnswer(groupContainingProgram0.Count, 169);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var groups = GetGroups();

        return new PuzzleAnswer(groups.Count, 179);
    }

    private List<List<int>> GetGroups()
    {
        var groups = new List<List<int>>();
        var programGroups = new Dictionary<int, List<int>>();

        foreach (var pipe in _pipes)
        {
            var list = new List<int> { pipe.ProgramId };
            groups.Add(list);
            programGroups.Add(pipe.ProgramId, list);
        }

        foreach (var pipe in _pipes)
        {
            var programGroup = programGroups[pipe.ProgramId];
            foreach (var connectedToProgramId in pipe.ConnectedToProgramIds)
            {
                var otherProgramGroup = programGroups[connectedToProgramId];
                if (programGroup != otherProgramGroup)
                {
                    // Merge other group to current
                    foreach (var programId in otherProgramGroup)
                    {
                        programGroups[programId] = programGroup;
                    }

                    programGroup.AddRange(otherProgramGroup);
                    groups.Remove(otherProgramGroup);
                }
            }
        }

        return groups;
    }
}