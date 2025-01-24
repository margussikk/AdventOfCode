namespace AdventOfCode.Year2017.Day25;
internal class State
{
    public char Name { get; private set; }

    public StateAction FalseAction { get; private set; } = new StateAction();

    public StateAction TrueAction { get; private set; } = new StateAction();

    public static State Parse(string[] lines)
    {
        var state = new State
        {
            Name = lines[0].Split([' ', '.'], StringSplitOptions.TrimEntries)[2][0],
            FalseAction = new StateAction(),
            TrueAction = new StateAction()
        };

        // Value 0
        if (!lines[1].Contains("If the current value is 0:"))
        {
            throw new InvalidOperationException("Failed to parse state 0 rule");
        }

        var index = IndexAfter(lines[2], "Write the value ");
        state.FalseAction.Write = lines[2][index] == '1';

        index = IndexAfter(lines[3], "Move one slot to the ");
        state.FalseAction.Movement = lines[3][index..^1] == "left" ? -1 : 1;

        index = IndexAfter(lines[4], "Continue with state ");
        state.FalseAction.NextStateName = lines[4][index];

        // Value 1
        if (!lines[5].Contains("If the current value is 1:"))
        {
            throw new InvalidOperationException("Failed to parse state 1 rule");
        }

        index = IndexAfter(lines[6], "Write the value ");
        state.TrueAction.Write = lines[6][index] == '1';

        index = IndexAfter(lines[7], "Move one slot to the ");
        state.TrueAction.Movement = lines[7][index..^1] == "left" ? -1 : 1;

        index = IndexAfter(lines[8], "Continue with state ");
        state.TrueAction.NextStateName = lines[8][index];

        return state;
    }

    private static int IndexAfter(string input, string pattern)
    {
        return input.IndexOf(pattern) + pattern.Length;
    }
}
