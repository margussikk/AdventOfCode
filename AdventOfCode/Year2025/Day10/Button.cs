namespace AdventOfCode.Year2025.Day10;

internal class Button
{
    public int WiringBitmask { get; }

    public int[] Wirings { get; }

    public Button(int buttonsCount, int[] buttons)
    {
        WiringBitmask = buttons.Sum(x => 1 << (buttonsCount - 1 - x));
        Wirings = buttons;
    }
}
