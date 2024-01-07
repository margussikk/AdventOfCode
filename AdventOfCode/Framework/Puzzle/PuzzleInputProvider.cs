using System.Text;

namespace AdventOfCode.Framework.Puzzle;

public static class PuzzleInputProvider
{
    public static bool TryGetInputLines(Type solverType, out List<string> lines)
    {
        lines = [];

        using var stream = solverType.Assembly.GetManifestResourceStream(solverType.Namespace + ".input.txt");
        if (stream != null)
        {
            string? line;

            using var reader = new StreamReader(stream, Encoding.UTF8);
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }

            return true;
        }
        else
        {
            return false;
        }
    }
}
