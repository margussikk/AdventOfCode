using System.Text;

namespace AdventOfCode.Utilities.Text;

internal static class Ocr
{
    private const string SmallLetters = "ABCEFGHIJKLOPRSUYZ";

    private static readonly string[] _smallLetterPatternLines =
    [
        " ##  ###   ##  #### ####  ##  #  #  ###   ## #  # #     ##  ###  ###   ### #  # #   ##### ",
        "#  # #  # #  # #    #    #  # #  #   #     # # #  #    #  # #  # #  # #    #  # #   #   # ",
        "#  # ###  #    ###  ###  #    ####   #     # ##   #    #  # #  # #  # #    #  #  # #   #  ",
        "#### #  # #    #    #    # ## #  #   #     # # #  #    #  # ###  ###   ##  #  #   #   #   ",
        "#  # #  # #  # #    #    #  # #  #   #  #  # # #  #    #  # #    # #     # #  #   #  #    ",
        "#  # ###   ##  #### #     ### #  #  ###  ##  #  # ####  ##  #    #  # ###   ##    #  #### "
    ];

    private const string LargeLetters = "ABCEFGHJKLNPRXZ";

    private static readonly string[] _largeLetterPatternLines =
    [
        "  ##    #####    ####   ######  ######   ####   #    #     ###  #    #  #       #    #  #####   #####   #    #  ######  ",
        " #  #   #    #  #    #  #       #       #    #  #    #      #   #   #   #       ##   #  #    #  #    #  #    #       #  ",
        "#    #  #    #  #       #       #       #       #    #      #   #  #    #       ##   #  #    #  #    #   #  #        #  ",
        "#    #  #    #  #       #       #       #       #    #      #   # #     #       # #  #  #    #  #    #   #  #       #   ",
        "#    #  #####   #       #####   #####   #       ######      #   ##      #       # #  #  #####   #####     ##       #    ",
        "######  #    #  #       #       #       #  ###  #    #      #   ##      #       #  # #  #       #  #      ##      #     ",
        "#    #  #    #  #       #       #       #    #  #    #      #   # #     #       #  # #  #       #   #    #  #    #      ",
        "#    #  #    #  #       #       #       #    #  #    #  #   #   #  #    #       #   ##  #       #   #    #  #   #       ",
        "#    #  #    #  #    #  #       #       #   ##  #    #  #   #   #   #   #       #   ##  #       #    #  #    #  #       ",
        "#    #  #####    ####   ######  #        ### #  #    #   ###    #    #  ######  #    #  #       #    #  #    #  ######  "
    ];

    public static string Parse(string text)
    {
        var textLines = text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToArray();
        if (textLines.Select(l => l.Length).Distinct().Count() != 1)
        {
            throw new InvalidOperationException("Different line lengths");
        }

        if (textLines.Length == _smallLetterPatternLines.Length)
        {
            // Small letters
            var smallLetterWidth = _smallLetterPatternLines[0].Length / SmallLetters.Length;
            if (textLines[0].Length % smallLetterWidth != 0)
            {
                throw new InvalidOperationException("Unexpected text width");
            }

            return ParseLetters(textLines, SmallLetters, _smallLetterPatternLines);
        }

        if (textLines.Length != _largeLetterPatternLines.Length)
            throw new InvalidOperationException("Unexpected text height");
        
        // Large letters
        var largeLetterWidth = _largeLetterPatternLines[0].Length / LargeLetters.Length;
        if (textLines[0].Length % largeLetterWidth != 0)
        {
            throw new InvalidOperationException("Unexpected text width");
        }

        return ParseLetters(textLines, LargeLetters, _largeLetterPatternLines);
    }

    private static string ParseLetters(string[] textLines, string letters, string[] patternLines)
    {
        var letterWidth = patternLines[0].Length / letters.Length;

        var stringBuilder = new StringBuilder();

        // Go through every block of pixels in the text
        for (var textPixelColumn = 0; textPixelColumn < textLines[0].Length; textPixelColumn += letterWidth)
        {
            var parsed = false;

            // Try to match it against every letter
            for (var letterIndex = 0; letterIndex < letters.Length && !parsed; letterIndex++)
            {
                var patternPixelColumn = letterIndex * letterWidth;
                var matched = true;

                // Check pixel by pixel if they match
                for (var row = 0; row < textLines.Length && matched; row++)
                {
                    for (var column = 0; column < letterWidth && matched; column++)
                    {
                        var isTextPixelLit = textLines[row][textPixelColumn + column] == '#';
                        var isPatternPixelLit = patternLines[row][patternPixelColumn + column] == '#';

                        if (isTextPixelLit != isPatternPixelLit)
                        {
                            matched = false;
                        }
                    }
                }

                if (!matched) continue;
                
                stringBuilder.Append(letters[letterIndex]);
                parsed = true;
            }

            if (!parsed)
            {
                stringBuilder.Append('?');
            }
        }

        return stringBuilder.ToString();
    }
}
