using System.Text;

namespace AdventOfCode.Year2019.IntCode;

internal class IntCodeResult(IntCodeExitCode exitCode, List<long> outputs)
{
    public IntCodeExitCode ExitCode { get; } = exitCode;

    public List<long> Outputs { get; } = outputs;

    public string GetAsciiOutput()
    {
        var stringBuilder = new StringBuilder();

        foreach (var character in Outputs)
        {
            stringBuilder.Append(Convert.ToChar(character));
        }

        return stringBuilder.ToString();
    }
}
