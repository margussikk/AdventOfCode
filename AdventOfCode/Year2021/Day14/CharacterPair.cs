namespace AdventOfCode.Year2021.Day14;

internal readonly struct CharacterPair : IEquatable<CharacterPair>
{
    public char Left { get; }
    public char Right { get; }

    public CharacterPair(char left, char right)
    {
        Left = left;
        Right = right;
    }

    public bool Equals(CharacterPair other)
    {
        return other.Left == Left && other.Right == Right;
    }

    public override bool Equals(object? obj) => obj is CharacterPair other && Equals(other);

    public override int GetHashCode() => (Left, Right).GetHashCode();

    public static bool operator ==(CharacterPair left, CharacterPair right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(CharacterPair left, CharacterPair right)
    {
        return !(left == right);
    }
}
