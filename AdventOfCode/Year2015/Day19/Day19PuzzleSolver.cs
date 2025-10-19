using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Collections;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2015.Day19;

[Puzzle(2015, 19, "Medicine for Rudolph")]
public class Day19PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Replacement> _replacements = [];
    private string[] _molecule = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        _replacements = [.. chunks[0].Select(Replacement.Parse)];
        _molecule = MoleculeHelper.Parse(chunks[1][0]);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var resultMolecules = new HashSet<ListKey<string>>();

        foreach (var replacement in _replacements)
        {
            var results = replacement.GenerateNewMolecules(_molecule, false);
            foreach (var result in results)
            {
                resultMolecules.Add(new ListKey<string>(result));
            }
        }

        var answer = resultMolecules.Count;

        return new PuzzleAnswer(answer, 576);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var queue = new PriorityQueue<State, int>();

        var state = new State
        {
            Molecule = _molecule,
            Steps = 0
        };

        queue.Enqueue(state, 0);

        var answer = 0;

        while (queue.TryDequeue(out state, out _))
        {
            if (state.Molecule.Length == 1 && state.Molecule[0] == "e")
            {
                answer = state.Steps;
                break;
            }

            foreach (var newMolecule in _replacements.SelectMany(r => r.GenerateNewMolecules(state.Molecule, true).Where(x => x.Length < state.Molecule.Length)))
            {
                var nextState = new State
                {
                    Molecule = newMolecule,
                    Steps = state.Steps + 1,
                };

                queue.Enqueue(nextState, newMolecule.Length + nextState.Steps);
            }
        }

        return new PuzzleAnswer(answer, 207);
    }
}