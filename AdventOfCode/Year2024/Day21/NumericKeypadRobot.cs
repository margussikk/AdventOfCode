using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2024.Day21;
internal class NumericKeypadRobot : Robot<char?>
{
    private static readonly Grid<char?> _keypad = BuildKeypad();

    public NumericKeypadRobot() : base(_keypad, 'A')
    {
    }

    public string PushButtons(IEnumerable<GridDirection> directions)
    {
        return new string(GenericPushButtons(directions).Cast<char>().ToArray());
    }

    public List<List<GridDirection>> GetDirections(Dictionary<(char?, char?), List<List<GridDirection>>> cache, string doorCode)
    {
        return GenericGetDirections(cache, doorCode.Cast<char?>().ToList(), 'A');
    }

    private static Grid<char?> BuildKeypad()
    {
        var keypad = new Grid<char?>(4, 3);
        keypad[0, 0] = '7';
        keypad[0, 1] = '8';
        keypad[0, 2] = '9';
        keypad[1, 0] = '4';
        keypad[1, 1] = '5';
        keypad[1, 2] = '6';
        keypad[2, 0] = '1';
        keypad[2, 1] = '2';
        keypad[2, 2] = '3';
        keypad[3, 0] = null;
        keypad[3, 1] = '0';
        keypad[3, 2] = 'A';

        return keypad;
    }
}
