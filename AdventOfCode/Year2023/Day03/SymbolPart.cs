namespace AdventOfCode.Year2023.Day03;

internal class SymbolPart(int row, int column, int length, char symbol) : EnginePart(row, column, length)
{
    public char Symbol { get; } = symbol;

    public bool IsGearPart => Symbol == '*';
}
