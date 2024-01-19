namespace AdventOfCode.Year2022.Day13;

internal class IntegerPacket(int value) : Packet
{
    public int Value { get; } = value;

    public override string ToString()
    {
        return Value.ToString();
    }

    public ListPacket ToListPacket()
    {
        var listPacket = new ListPacket();
        listPacket.AddPacket(new IntegerPacket(Value));

        return listPacket;
    }

    public override int CompareTo(IntegerPacket integerPacket)
    {
        return Value.CompareTo(integerPacket.Value);
    }

    public override int CompareTo(ListPacket listPacket)
    {
        return ToListPacket().CompareTo(listPacket);
    }
}
