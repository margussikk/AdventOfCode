namespace AdventOfCode.Year2019.Day12;
internal class Moon
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }


    public int VX { get; set; }
    public int VY { get; set; }
    public int VZ { get; set; }

    public Moon Clone()
    {
        return new Moon
        {
            X = X,
            Y = Y,
            Z = Z,

            VX = VX,
            VY = VY,
            VZ = VZ
        };    
    }

    public void ApplyGravity(Moon other)
    {
        if (X != other.X)
        {
            VX += Math.Sign(other.X - X);
        }

        if (Y != other.Y)
        {
            VY += Math.Sign(other.Y - Y);
        }

        if (Z != other.Z)
        {
            VZ += Math.Sign(other.Z - Z);
        }
    }
    public int GetTotalEnergy()
    {
        var potentialEnergy = Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
        var kineticEnergy = Math.Abs(VX) + Math.Abs(VY) + Math.Abs(VZ);

        return potentialEnergy * kineticEnergy;
    }

    public static Moon Parse(string input)
    {
        var separators = new []
        {
            "<x=", ", y=", ", z=", ">"
        };

        var splits = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        var moon = new Moon
        {
            X = int.Parse(splits[0]),
            Y = int.Parse(splits[1]),
            Z = int.Parse(splits[2]),
            VX = 0,
            VY = 0,
            VZ = 0
        };

        return moon;
    }
}
