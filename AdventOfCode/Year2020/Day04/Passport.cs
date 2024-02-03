namespace AdventOfCode.Year2020.Day04;

internal class Passport
{
    private static readonly string[] _validEyeColors = ["amb", "blu", "brn", "gry", "grn", "hzl", "oth"];

    public string? BirthYear { get; private set; }

    public string? IssueYear { get; private set; }

    public string? ExpirationYear { get; private set; }

    public string? Height { get; private set; }

    public string? HairColor { get; private set; }

    public string? EyeColor { get; private set; }

    public string? PassportID { get; private set; }

    public string? CountryID { get; private set; }

    public bool RequiredFieldsAreFilled()
    {
        if (BirthYear is null ||
            IssueYear is null ||
            ExpirationYear is null ||
            Height is null ||
            HairColor is null ||
            EyeColor is null ||
            PassportID is null)
        {
            return false;
        }

        return true;
    }

    public bool IsDataValid()
    {
        if (BirthYear is null ||
            IssueYear is null ||
            ExpirationYear is null ||
            Height is null ||
            HairColor is null ||
            EyeColor is null ||
            PassportID is null)
        {
            return false;
        }

        // Birth Year
        var birthYear = int.Parse(BirthYear);
        if (birthYear < 1920 || birthYear > 2002)
        {
            return false;
        }

        // Issue Year
        var issueYear = int.Parse(IssueYear);
        if (issueYear < 2010 || issueYear > 2020)
        {
            return false;
        }

        // Expiration Year
        var expirationYear = int.Parse(ExpirationYear);
        if (expirationYear < 2020 || expirationYear > 2030)
        {
            return false;
        }

        // Height
        if (Height.EndsWith("cm"))
        {
            var height = int.Parse(Height[..^2]);
            if (height < 150 || height > 193)
            {
                return false;
            }
        }
        else if (Height.EndsWith("in"))
        {
            var height = int.Parse(Height[..^2]);
            if (height < 59 || height > 76)
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        // Hair Color
        if (HairColor[0] == '#')
        {
            var hairColor = HairColor[1..];
            if (hairColor.Length != 6)
            {
                return false;
            }

            if (hairColor.Any(character =>
                (character < '0' || character > '9') &&
                (character < 'a' || character > 'f')))
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        // Eye Color
        if (!_validEyeColors.Contains(EyeColor))
        {
            return false;
        }

        // Passport ID
        if (PassportID.Length == 9)
        {
            if (!PassportID.All(char.IsDigit))
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    public static Passport Parse(string[] lines)
    {
        var passport = new Passport();

        var keyValuePairs = lines
            .SelectMany(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .ToList();

        foreach (var keyValuePair in keyValuePairs)
        {
            var splits = keyValuePair.Split(':');

            switch (splits[0])
            {
                case "byr":
                    passport.BirthYear = splits[1];
                    break;
                case "iyr":
                    passport.IssueYear = splits[1];
                    break;
                case "eyr":
                    passport.ExpirationYear = splits[1];
                    break;
                case "hgt":
                    passport.Height = splits[1];
                    break;
                case "hcl":
                    passport.HairColor = splits[1];
                    break;
                case "ecl":
                    passport.EyeColor = splits[1];
                    break;
                case "pid":
                    passport.PassportID = splits[1];
                    break;
                case "cid":
                    passport.CountryID = splits[1];
                    break;
                default:
                    throw new InvalidOperationException($"Invalid field key {splits[0]}");
            }
        }

        return passport;
    }
}
