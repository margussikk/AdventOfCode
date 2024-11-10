namespace AdventOfCode.Year2022.Day02;

internal class Round
{
    public Hand OpponentHand { get; private init; }

    public Decision PlayerDecision { get; private init; }

    public int CalculateScorePartOne()
    {
        var playerHand = PlayerDecision switch
        {
            Decision.X => Hand.Rock,
            Decision.Y => Hand.Paper,
            Decision.Z => Hand.Scissors,
            _ => throw new NotImplementedException()
        };

        return CalculateScore(playerHand, OpponentHand);
    }

    public int CalculateScorePartTwo()
    {
        var playerHand = PlayerDecision switch
        {
            Decision.X => OpponentHand switch // Lose
            {
                Hand.Rock => Hand.Scissors,
                Hand.Paper => Hand.Rock,
                Hand.Scissors => Hand.Paper,
                _ => throw new NotImplementedException()
            },
            Decision.Y => OpponentHand, // Draw
            Decision.Z => OpponentHand switch // Win
            {
                Hand.Rock => Hand.Paper,
                Hand.Paper => Hand.Scissors,
                Hand.Scissors => Hand.Rock,
                _ => throw new NotImplementedException()
            },
            _ => throw new NotImplementedException()
        };

        return CalculateScore(playerHand, OpponentHand);
    }

    public static Round Parse(string input)
    {
        var splits = input.Split(' ');

        var round = new Round
        {
            OpponentHand = splits[0][0] switch
            {
                'A' => Hand.Rock,
                'B' => Hand.Paper,
                'C' => Hand.Scissors,
                _ => throw new NotImplementedException()
            },
            PlayerDecision = Enum.Parse<Decision>(splits[1])
        };

        return round;
    }

    private static int CalculateScore(Hand playerHand, Hand opponentHand)
    {
        var score = playerHand switch
        {
            Hand.Rock => 1,
            Hand.Paper => 2,
            Hand.Scissors => 3,
            _ => throw new NotImplementedException()
        };

        if (playerHand == opponentHand)
        {
            score += 3;
        }
        else if ((playerHand == Hand.Rock && opponentHand == Hand.Scissors) ||
                (playerHand == Hand.Paper && opponentHand == Hand.Rock) ||
                (playerHand == Hand.Scissors && opponentHand == Hand.Paper))
        {
            score += 6;
        }

        return score;
    }
}
