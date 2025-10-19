namespace AdventOfCode.Year2015.Day19;

internal class Replacement
{
    public string[] OldMoleculePart { get; private set; } = [];
    public string[] NewMoleculePart { get; private set; } = [];

    public List<string[]> GenerateNewMolecules(string[] inputMolecule, bool reverse)
    {
        if (reverse)
        {
            return MoleculeHelper.GenerateNewMolecules(inputMolecule, NewMoleculePart, OldMoleculePart);
        }

        return MoleculeHelper.GenerateNewMolecules(inputMolecule, OldMoleculePart, NewMoleculePart);
    }

    public override string ToString()
    {
        return $"{OldMoleculePart} => {string.Join("", NewMoleculePart)}";
    }

    public static Replacement Parse(string line)
    {
        var parts = line.Split(" => ");
        return new Replacement
        {
            OldMoleculePart = MoleculeHelper.Parse(parts[0]),
            NewMoleculePart = MoleculeHelper.Parse(parts[1])
        };
    }
}
