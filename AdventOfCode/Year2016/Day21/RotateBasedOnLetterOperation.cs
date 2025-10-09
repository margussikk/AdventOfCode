using AdventOfCode.Utilities.Mathematics;

namespace AdventOfCode.Year2016.Day21;

internal class RotateBasedOnLetterOperation : Operation
{
    public char Letter { get; init; }

    // 0 -> 1 | +1
    // 1 -> 3 | +2
    // 2 -> 5 | +3
    // 3 -> 7 | +4
    // 4 -> 2 | +6
    // 5 -> 4 | +7
    // 6 -> 6 | +8, 0
    // 7 -> 0 | +9, +1
    public override void Scramble(char[] letters)
    {
        var index = Array.IndexOf(letters, Letter);
        var steps = 1 + index;
        if (index >= 4)
        {
            steps++;
        }

        var clone = letters.ToArray();

        for (var position = 0; position < letters.Length; position++)
        {
            letters[position] = clone[MathFunctions.Modulo(letters.Length + position - steps, letters.Length)];
        }
    }

    // 0 -> 7 | -1
    // 1 -> 0 | -1
    // 2 -> 4 | -6
    // 3 -> 1 | -2
    // 4 -> 5 | -7
    // 5 -> 2 | -3
    // 6 -> 6 | -8, 0
    // 7 -> 3 | -4
    public override void Unscramble(char[] letters)
    {
        var index = Array.IndexOf(letters, Letter);
        var steps = (index + ((index % 2 != 0 || index == 0) ? 2 : 10)) / 2;

        RotateLeft(letters, steps);
    }

    public void UnscrambleUsingBruteForce(char[] letters)
    {
        for (var steps = 0; steps < letters.Length; steps++)
        {
            var unscrambledLetters = letters.ToArray();
            RotateLeft(unscrambledLetters, steps);

            var scrambledLetters = unscrambledLetters.ToArray();
            Scramble(scrambledLetters);

            if (letters.SequenceEqual(scrambledLetters))
            {
                for (var position = 0; position < letters.Length; position++)
                {
                    letters[position] = unscrambledLetters[position];
                }
                break;
            }
        }
    }

    private static void RotateLeft(char[] letters, int steps)
    {
        var rotateOperation = new RotateStepsOperation
        {
            Steps = -steps,
        };

        rotateOperation.Scramble(letters);
    }
}
