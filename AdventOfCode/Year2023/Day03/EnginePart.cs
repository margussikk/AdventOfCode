namespace AdventOfCode.Year2023.Day03;

internal abstract class EnginePart(int row, int column, int length)
{
    public int Row { get; } = row;

    public int Column { get; } = column;

    public int Length { get; } = length;

    public bool IsAdjacentTo(EnginePart other)
    {
        var topAdjacentRow = Row - 1;
        var bottomAdjacentRow = Row + 1;
        var leftAdjacentColumn = Column - 1;
        var rightAdjacentColumn = Column + Length;

        var otherRightColumn = other.Column + other.Length - 1;

        return other.Row >= topAdjacentRow && // Top
               other.Row <= bottomAdjacentRow && // Bottom
               ((other.Column >= leftAdjacentColumn && other.Column <= rightAdjacentColumn) || // Start: Left and Right
                (otherRightColumn >= leftAdjacentColumn && otherRightColumn <= rightAdjacentColumn)); // End: Left and Right

    }
}

