namespace AdventOfCode.Year2022.Day13;

internal abstract class Packet : IComparable<Packet>
{
    public int CompareTo(Packet? other)
    {
        return other switch
        {
            ListPacket listPacket => CompareTo(listPacket),
            IntegerPacket integerPacket => CompareTo(integerPacket),
            _ => throw new InvalidOperationException()
        };
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Packet other)
        {
            return false;
        }

        return CompareTo(other) == 0;
    }

    public static bool operator ==(Packet? left, Packet? right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    public static bool operator !=(Packet? left, Packet? right)
    {
        return !(left == right);
    }

    public static bool operator >(Packet left, Packet right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(Packet left, Packet right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static bool operator <(Packet left, Packet right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(Packet left, Packet right)
    {
        return left.CompareTo(right) <= 0;
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

    public abstract int CompareTo(ListPacket listPacket);

    public abstract int CompareTo(IntegerPacket integerPacket);

    public static ListPacket Parse(string line)
    {
        var stack = new Stack<ListPacket>();
        ListPacket? currentPacket = null;

        for (var index = 0; index < line.Length; index++)
        {
            if (line[index] == '[')
            {
                var newPacket = new ListPacket();

                if (currentPacket != null)
                {
                    currentPacket.AddPacket(newPacket);
                    stack.Push(currentPacket);
                }

                currentPacket = newPacket;
            }
            else if (char.IsDigit(line[index]) && currentPacket != null)
            {
                var startIndex = index;

                while (char.IsDigit(line[index + 1]))
                {
                    index++;
                }

                var value = int.Parse(line.Substring(startIndex, index - startIndex + 1));
                var packet = new IntegerPacket(value);

                currentPacket.AddPacket(packet);
            }
            else if (line[index] == ',' && currentPacket != null)
            {
                // Ignore
            }
            else if (line[index] == ']' && currentPacket != null)
            {
                if (stack.Count != 0)
                {
                    currentPacket = stack.Pop();
                }
                else
                {
                    return currentPacket;
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        throw new InvalidOperationException();
    }
}
