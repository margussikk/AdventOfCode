namespace AdventOfCode.Year2017.Day16;

internal class SpinDanceMove : IDanceMove
{
    public int Count { get; private set; }

    public void DoMove(List<char> programs)
    {
        for (var i = 0; i < Count; i++)
        {
            var temp = programs[^1];
            programs.RemoveAt(programs.Count - 1);
            programs.Insert(0, temp);
        }
    }

    public static SpinDanceMove Parse(string input)
    {
        if (input[0] != 's')
        {
            throw new InvalidOperationException("Spin dance move must start with s");
        }

        return new SpinDanceMove
        {
            Count = int.Parse(input[1..])
        };
    }
}
