namespace AdventOfCode.Year2021.Day21;

internal readonly struct GameState : IEquatable<GameState>
{
    public int ActivePlayerPosition { get; }
    public int ActivePlayerScore { get; }
    public int IdlePlayerPosition { get; }
    public int IdlePlayerScore { get; }

    public GameState(int activePlayerPosition, int activePlayerScore, int idlePlayerPosition, int idlePlayerScore)
    {
        ActivePlayerPosition = activePlayerPosition;
        ActivePlayerScore = activePlayerScore;
        IdlePlayerPosition = idlePlayerPosition;
        IdlePlayerScore = idlePlayerScore;
    }
    
    public bool Equals(GameState other)
    {
        return other.ActivePlayerPosition == ActivePlayerPosition
            && other.ActivePlayerScore == ActivePlayerScore
            && other.IdlePlayerPosition == IdlePlayerPosition
            && other.IdlePlayerScore == IdlePlayerScore;
    }

    public override bool Equals(object? obj) => obj is GameState other && Equals(other);

    public override int GetHashCode() => (ActivePlayerPosition, IdlePlayerPosition, ActivePlayerScore, IdlePlayerScore).GetHashCode();

    public static bool operator ==(GameState left, GameState right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(GameState left, GameState right)
    {
        return !(left == right);
    }
}
