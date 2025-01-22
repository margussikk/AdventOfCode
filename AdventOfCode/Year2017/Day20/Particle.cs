using AdventOfCode.Utilities.Geometry;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2017.Day20;
internal partial class Particle
{
    public Coordinate3D Position { get; private set; }

    public Vector3D Velocity { get; private set; }

    public Vector3D Acceleration { get; private set; }

    public Particle Clone()
    {
        return new Particle
        {
            Position = Position,
            Velocity = Velocity,
            Acceleration = Acceleration
        };
    }

    public bool IsVelocityIncreasing()
    {
        if (Math.Sign(Velocity.DX) * Math.Sign(Acceleration.DX) == -1)
        {
            return false;
        }

        if (Math.Sign(Velocity.DY) * Math.Sign(Acceleration.DY) == -1)
        {
            return false;
        }

        if (Math.Sign(Velocity.DZ) * Math.Sign(Acceleration.DZ) == -1)
        {
            return false;
        }

        return true;
    }

    public void Tick()
    {
        Velocity += Acceleration;
        Position += Velocity;
    }

    public static Particle Parse(string input)
    {
        var matches = InputLineRegex().Matches(input);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse input line");
        }

        var match = matches[0];

        var particle = new Particle
        {
            Position = Coordinate3D.Parse(match.Groups[1].Value),
            Velocity = Vector3D.Parse(match.Groups[2].Value),
            Acceleration = Vector3D.Parse(match.Groups[3].Value)
        };

        return particle;
    }

    [GeneratedRegex(@"p=<(\s*-?\d+,\s*-?\d+,\s*-?\d+)>, v=<(\s*-?\d+,\s*-?\d+,\s*-?\d+)>, a=<(\s*-?\d+,\s*-?\d+,\s*-?\d+)>")]
    private static partial Regex InputLineRegex();
}
