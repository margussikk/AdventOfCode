namespace AdventOfCode.Year2021.Day21;

internal struct GameState(int player1Position, int player1Score, int player2Position, int player2Score) : IEquatable<GameState>
{
    public int ActivePlayerPosition { get; set; } = player1Position;
    public int IdlePlayerPosition { get; set; } = player2Position;
    public int ActivePlayerScore { get; set; } = player1Score;
    public int IdlePlayerScore { get; set; } = player2Score;

    public readonly bool Equals(GameState other)
    {
        return other.ActivePlayerPosition == ActivePlayerPosition
            && other.ActivePlayerScore == ActivePlayerScore
            && other.IdlePlayerPosition == IdlePlayerPosition
            && other.IdlePlayerScore == IdlePlayerScore;
    }

    public override readonly bool Equals(object? obj) => obj is GameState other && Equals(other);

    public override readonly int GetHashCode() => (ActivePlayerPosition, IdlePlayerPosition, ActivePlayerScore, IdlePlayerScore).GetHashCode();

    public static bool operator ==(GameState left, GameState right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(GameState left, GameState right)
    {
        return !(left == right);
    }
}
