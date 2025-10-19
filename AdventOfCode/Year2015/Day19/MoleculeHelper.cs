namespace AdventOfCode.Year2015.Day19;

internal static class MoleculeHelper
{
    public static List<string[]> GenerateNewMolecules(string[] molecule, string[] oldMoleculePart, string[] newMoleculePart)
    {
        var newMolecules = new List<string[]>();

        for (var index = 0; index < molecule.Length; index++)
        {
            for (var index2 = 0; index2 < oldMoleculePart.Length; index2++)
            {
                if (index + oldMoleculePart.Length - 1 >= molecule.Length)
                {
                    break;
                }

                if (molecule[index + index2] != oldMoleculePart[index2])
                {
                    break;
                }

                if (index2 == oldMoleculePart.Length - 1)
                {
                    newMolecules.Add([.. molecule[..index], .. newMoleculePart, .. molecule[(index + oldMoleculePart.Length)..]]);
                }
            }
        }

        return newMolecules;
    }

    public static string[] Parse(string input)
    {
        var elements = new List<string>();

        for (var i = 0; i < input.Length; i++)
        {
            var ch = input[i];
            if (char.IsUpper(ch) || ch == 'e')
            {
                elements.Add(ch.ToString());
            }
            else
            {
                elements[^1] = $"{elements[^1]}{ch}";
            }
        }

        return [.. elements];
    }
}
