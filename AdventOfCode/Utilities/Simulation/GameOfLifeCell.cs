using AdventOfCode.Utilities.GridSystem;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Utilities.Simulation;

internal class GameOfLifeCell<T> where T : Enum
{
    private const int BITMASK = 0x0F;

    private readonly int _value;

    public GridCoordinate Coordinate { get; }

    public T Object { get; }

    public GameOfLifeCell(GridCoordinate coordinate, int value)
    {
        Coordinate = coordinate;
        Object = Unsafe.BitCast<int, T>(value & BITMASK);

        _value = value;
    }

    public int AroundCount(T value)
    {
        return (_value >> (4 * Unsafe.BitCast<T, int>(value))) & BITMASK;
    }
}
