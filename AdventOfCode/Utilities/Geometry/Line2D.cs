using System.Numerics;
using System.Text;

namespace AdventOfCode.Utilities.Geometry;

// Standard form Ax + By + C = 0
internal readonly struct Line2D
{
    public long A { get; }

    public long B { get; }

    public long C { get; }

    public Line2D(long a, long b, long c)
    {
        A = a;
        B = b;
        C = c;

        if (A >= 0) return;
        
        A = -A;
        B = -B;
        C = -C;
    }

    public Line2D(Coordinate2D coordinate1, Coordinate2D coordinate2)
    {
        A = coordinate1.Y - coordinate2.Y;
        B = coordinate2.X - coordinate1.X;

        var bigC = new BigInteger(coordinate1.X) * new BigInteger(coordinate2.Y) +
                   new BigInteger(coordinate2.X) * new BigInteger(coordinate2.Y) -
                   new BigInteger(coordinate2.X) * new BigInteger(coordinate1.Y) -
                   new BigInteger(coordinate2.X) * new BigInteger(coordinate2.Y);
        C = (long)bigC;

        if (A >= 0) return;
        
        A = -A;
        B = -B;
        C = -C;
    }

    public Line2D(Coordinate2D coordinate, Vector2D vector)
    {
        A = -vector.DY;
        B = vector.DX;

        var bigC = new BigInteger(vector.DY) * new BigInteger(coordinate.X) -
                   new BigInteger(coordinate.Y) * new BigInteger(vector.DX);
        C = (long)bigC;

        if (A >= 0) return;
        
        A = -A;
        B = -B;
        C = -C;
    }

    public bool TryFindIntersectionCoordinate(Line2D other, out Coordinate2D intersectionCoordinate)
    {
        var bigA = new BigInteger(A);
        var bigOtherA = new BigInteger(other.A);

        var bigB = new BigInteger(B);
        var bigOtherB = new BigInteger(other.B);

        var bigC = new BigInteger(C);
        var bigOtherC = new BigInteger(other.C);

        var determinant = bigA * bigOtherB - bigB * bigOtherA;
        if (determinant == 0L) // Parallel lines
        {
            intersectionCoordinate = default;
            return false;
        }

        var x = (bigB * bigOtherC - bigC * bigOtherB) / determinant;
        var y = (bigC * bigOtherA - bigA * bigOtherC) / determinant;

        intersectionCoordinate = new Coordinate2D((long)x, (long)y);
        return true;
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        if (A != 0L)
        {
            if (Math.Abs(A) != 1)
            {
                stringBuilder.Append(A);
            }

            stringBuilder.Append('x');
        }

        if (B != 0)
        {
            stringBuilder.Append(B >= 0 ? " + " : " - ");

            var value = Math.Abs(B);
            if (value != 1)
            {
                stringBuilder.Append(value);
            }

            stringBuilder.Append('y');
        }

        if (C != 0)
        {
            stringBuilder.Append(C >= 0 ? " + " : " - ");

            stringBuilder.Append(Math.Abs(C));
        }

        stringBuilder.Append(" = 0");

        return stringBuilder.ToString();
    }
}
