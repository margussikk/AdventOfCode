namespace AdventOfCode.Year2016.Day21;

internal abstract class Operation
{
    public abstract void Scramble(char[] letters);

    public abstract void Unscramble(char[] letters);

    public static Operation Parse(string input)
    {
        var parts = input.Split(' ');

        if (parts[0] == "swap" && parts[1] == "position")
        {
            return new SwapPositionOperation
            {
                Position1 = int.Parse(parts[2]),
                Position2 = int.Parse(parts[5]),
            };
        }
        else if (parts[0] == "swap" && parts[1] == "letter")
        {
            return new SwapLetterOperation
            {
                Letter1 = parts[2][0],
                Letter2 = parts[5][0],
            };
        }
        else if (parts[0] == "reverse" && parts[1] == "positions")
        {
            return new ReversePositionsOperation
            {
                Position1 = int.Parse(parts[2]),
                Position2 = int.Parse(parts[4]),
            };
        }
        else if (parts[0] == "rotate" && (parts[1] == "left" || parts[1] == "right"))
        {
            var sign = parts[1] == "left" ? -1 : 1;

            return new RotateStepsOperation
            {
                Steps = sign * int.Parse(parts[2]),
            };
        }
        else if (parts[0] == "rotate" && parts[1] == "based")
        {
            return new RotateBasedOnLetterOperation
            {
                Letter = parts[6][0],
            };
        }
        else if (parts[0] == "move" && parts[1] == "position")
        {
            return new MovePositionOperation
            {
                StartPosition = int.Parse(parts[2]),
                EndPosition = int.Parse(parts[5]),
            };
        }


        throw new InvalidOperationException($"Failed to parse input: {input}");
    }
}
