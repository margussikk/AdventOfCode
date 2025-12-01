namespace AdventOfCode.Year2017.Day16;

internal class PartnerDanceMove : IDanceMove
{
    public char ProgramA { get; private set; }
    public char ProgramB { get; private set; }

    public void DoMove(List<char> programs)
    {
        var indexA = programs.IndexOf(ProgramA);
        var indexB = programs.IndexOf(ProgramB);

        (programs[indexB], programs[indexA]) = (programs[indexA], programs[indexB]);
    }

    public static PartnerDanceMove Parse(string input)
    {
        if (input[0] != 'p')
        {
            throw new InvalidOperationException("Partner dance move must start with p");
        }

        var splits = input[1..].Split('/');

        return new PartnerDanceMove
        {
            ProgramA = splits[0][0],
            ProgramB = splits[1][0],
        };
    }
}
