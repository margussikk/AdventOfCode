namespace AdventOfCode.Year2025.Day10;

internal class ButtonWiring
{
    public int Bitmask { get; }

    public int[] Buttons { get; }

    public ButtonWiring(int buttonsCount, int[] buttons)
    {

        Bitmask = buttons.Sum(x => 1 << (buttonsCount - 1 - x));
        Buttons = buttons;
    }
}
