using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2017.Day07;
internal class Program
{
    public string Name { get; }

    public int Weight { get; set; }

    public Program? BelowProgram { get; set; }
    public List<Program> AbovePrograms { get; } = [];

    public Program(string name)
    {
        Name = name;
    }

    public int GetEffectiveWeight()
    {
        return AbovePrograms.Sum(p => p.GetEffectiveWeight()) + Weight;
    }

    public (bool Found, int Weight) FindIncorrectWeight()
    {
        var aboveProgramWeights = new Dictionary<int, List<Program>>();

        foreach (var aboveProgram in AbovePrograms)
        {
            var result = aboveProgram.FindIncorrectWeight();
            if (result.Found)
            {
                return result;
            }

            aboveProgramWeights.AddToValueList(result.Weight, aboveProgram);
        }

        if (aboveProgramWeights.Count == 0)
        {
            return (false, Weight);
        }
        else if (aboveProgramWeights.Count == 1)
        {
            var kvp = aboveProgramWeights.First();
            return (false, kvp.Key * kvp.Value.Count + Weight);
        }
        else if (aboveProgramWeights.Count != 2)
        {
            throw new InvalidOperationException();
        }

        var correctWeight = aboveProgramWeights.First(x => x.Value.Count != 1).Key;
        var incorrectWeight = aboveProgramWeights.First(x => x.Value.Count == 1).Key;
        var incorrectProgram = aboveProgramWeights[incorrectWeight][0];
        var adjustment = correctWeight - incorrectWeight;

        var fixedWeigth = incorrectProgram.Weight + adjustment;

        return (true, fixedWeigth);
    }
}
