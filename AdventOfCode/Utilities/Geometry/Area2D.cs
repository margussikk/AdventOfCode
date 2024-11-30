using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Utilities.Geometry
{
    internal class Area2D : IEnumerable<Coordinate2D>
    {
        public Coordinate2D MinCoordinate { get; }

        public Coordinate2D MaxCoordinate { get; }

        public long XLength { get; }

        public long YLength { get; }

        public Area2D(Coordinate2D minCoordinate, Coordinate2D maxCoordinate)
        {
            MinCoordinate = minCoordinate;
            MaxCoordinate = maxCoordinate;

            XLength = MaxCoordinate.X - MinCoordinate.X + 1;
            YLength = MaxCoordinate.Y - MinCoordinate.Y + 1;
        }

        public bool TryFindOverlap(Area2D other, [MaybeNullWhen(false)] out Area2D overlapArea)
        {
            if (Overlaps(other))
            {
                var minCoordinate = new Coordinate2D(Math.Max(MinCoordinate.X, other.MinCoordinate.X), Math.Max(MinCoordinate.Y, other.MinCoordinate.Y));
                var maxCoordinate = new Coordinate2D(Math.Min(MaxCoordinate.X, other.MaxCoordinate.X), Math.Min(MaxCoordinate.Y, other.MaxCoordinate.Y));

                overlapArea = new Area2D(minCoordinate, maxCoordinate);

                return true;
            }

            overlapArea = null;
            return false;
        }

        public bool Overlaps(Area2D other)
        {
            return MinCoordinate.X <= other.MaxCoordinate.X &&
                   MaxCoordinate.X >= other.MinCoordinate.X &&
                   MinCoordinate.Y <= other.MaxCoordinate.Y &&
                   MaxCoordinate.Y >= other.MinCoordinate.Y;
        }

        public IEnumerator<Coordinate2D> GetEnumerator()
        {
            for (var y = MinCoordinate.Y; y <= MaxCoordinate.Y; y++)
            {
                for (var x = MinCoordinate.X; x <= MaxCoordinate.X; x++)
                {
                    yield return new Coordinate2D(x, y);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
