using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2024.Day21;
internal class DirectionalKeypadRobot : Robot<GridDirection?>
{
    private static readonly Grid<GridDirection?> _keypad = BuildKeypad();

    public DirectionalKeypadRobot() : base(_keypad, GridDirection.Start)
    {
    }

    public List<GridDirection> PushButtons(IEnumerable<GridDirection> directions)
    {
        return GenericPushButtons(directions).Cast<GridDirection>().ToList();
    }

    public List<List<GridDirection>> GetDirections(Dictionary<(GridDirection?, GridDirection?), List<List<GridDirection>>> cache, IEnumerable<GridDirection> pushedButtons)
    {
        return GenericGetDirections(cache, pushedButtons.Cast<GridDirection?>().ToList(), GridDirection.Start);
    }

    private static Grid<GridDirection?> BuildKeypad()
    {
        var keypad = new Grid<GridDirection?>(2, 3);
        keypad[0, 0] = null;
        keypad[0, 1] = GridDirection.Up;
        keypad[0, 2] = GridDirection.Start;
        keypad[1, 0] = GridDirection.Left;
        keypad[1, 1] = GridDirection.Down;
        keypad[1, 2] = GridDirection.Right;

        return keypad;
    }
}
