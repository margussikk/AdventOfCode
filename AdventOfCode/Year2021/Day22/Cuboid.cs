namespace AdventOfCode.Year2021.Day22;

internal class Cuboid
{
    public long X1 { get; }
    public long X2 { get; }
    public long Y1 { get; }
    public long Y2 { get; }
    public long Z1 { get; }
    public long Z2 { get; }

    public bool On { get; }

    public long Count => (On ? 1 : -1) * (X2 - X1 + 1) * (Y2 - Y1 + 1) * (Z2 - Z1 + 1);

    public Cuboid(long x1, long x2, long y1, long y2, long z1, long z2, bool on)
    {
        X1 = x1;
        X2 = x2;
        Y1 = y1;
        Y2 = y2;
        Z1 = z1;
        Z2 = z2;
        On = on;
    }
    
    public bool Intersects(Cuboid other)
    {
        return !(other.X2 < X1 || other.X1 > X2 ||
            other.Y2 < Y1 || other.Y1 > Y2 ||
            other.Z2 < Z1 || other.Z1 > Z2);
    }

    public Cuboid Intersection(Cuboid other)
    {
        return new Cuboid(Math.Max(X1, other.X1), Math.Min(X2, other.X2),
                          Math.Max(Y1, other.Y1), Math.Min(Y2, other.Y2),
                          Math.Max(Z1, other.Z1), Math.Min(Z2, other.Z2), !On); // Flip 'On' to not doubly count this area
    }

    //              a-------------------------b             a = (X1, Y2, Z2), b = (X2, Y2, Z2)
    //             /.                        /|             c = (X1, Y2, Z1), d = (X2, Y2, Z1)
    //            / .                       / |             e = (X1, Y1, Z2), f = (X2, Y1, Z2)
    //           /  .                      /  |             g = (X1, Y1, Z1), h = (X2, Y1, Z1)
    //          /   .                     /   |
    //         /    .                    /    |             k = (o.X1, o.Y2, o.Z2), l = (o.X2, o.Y2, o.Z2)
    //        c-------------------------d     |             m = (o.X1, o.Y2, o.Z1), n = (o.X2, o.Y2, o.Z1)
    //        |     .                   |     |             o = (o.X1, o.Y1, o.Z2), p = (o.X2, o.Y1, o.Z2)
    //        |     .    k--------l     |     |             q = (o.X1, o.Y1, o.Z1), r = (o.X2, o.Y1, o.Z1)
    //        |     .   /|       /|     |     |
    //        |     .  m--------n |     |     |             Image shows the case when the other cuboid is entirely inside the first cuboid,
    //        |     .  | |      | |     |     |             in that case entire another cuboid is the intersection and substraction creates 6 new cuboids.
    //        |     e..|.|......|.|.....|.....f
    //        |    .   | |      | |     |    /
    //        |   .    | o------|-p     |   /
    //        |  .     |/       |/      |  /
    //        | .      q--------r       | / 
    //        |.                        |/     
    //        g-------------------------h
    public List<Cuboid> Split(Cuboid other)
    {
        var cuboids = new List<Cuboid>();

        var intersection = new Cuboid(Math.Max(X1, other.X1), Math.Min(X2, other.X2),
                                      Math.Max(Y1, other.Y1), Math.Min(Y2, other.Y2),
                                      Math.Max(Z1, other.Z1), Math.Min(Z2, other.Z2), true);

        if (Z1 < intersection.Z1)
        {
            // Front part. After slicing through the q-m-n-r panel, take the front part
            cuboids.Add(new Cuboid(X1, X2, Y1, Y2, Z1, intersection.Z1 - 1, true));
        }

        if (intersection.Z2 < Z2)
        {
            // Back part. After slicing through the o-k-l-p panel, take the back part
            cuboids.Add(new Cuboid(X1, X2, Y1, Y2, intersection.Z2 + 1, Z2, true));
        }

        if (X1 < intersection.X1)
        {
            // Left part. After slicing through panels o-k-l-p, q-m-n-r and q-m-k-o, take the left part
            cuboids.Add(new Cuboid(X1, intersection.X1 - 1, Y1, Y2, intersection.Z1, intersection.Z2, true));
        }

        if (intersection.X2 < X2)
        {
            // Right part. After slicing through panels o-k-l-p, q-m-n-r and r-n-l-p, take the right part
            cuboids.Add(new Cuboid(intersection.X2 + 1, X2, Y1, Y2, intersection.Z1, intersection.Z2, true));
        }

        if (intersection.Y2 < Y2)
        {
            // Top part. After slicing through panels o-k-l-p, q-m-n-r, q-m-k-o and r-n-l-p, take the top part
            cuboids.Add(new Cuboid(intersection.X1, intersection.X2, intersection.Y2 + 1, Y2, intersection.Z1, intersection.Z2, true));
        }

        if (Y1 < intersection.Y1)
        {
            // Bottom part. After slicing through panels o-k-l-p, q-m-n-r, q-m-k-o and r-n-l-p, take the bottom part
            cuboids.Add(new Cuboid(intersection.X1, intersection.X2, Y1, intersection.Y1 - 1, intersection.Z1, intersection.Z2, true));
        }

        return cuboids;
    }

    public override string ToString()
    {
        return $"{(On ? "on" : "off")} x={X1}..{X2},y={Y1}..{Y2},z={Z1}..{Z2}";
    }
}
