namespace AdventOfCode.Year2022.Day13;

internal class ListPacket : Packet
{
    public List<Packet> Packets { get; }

    public ListPacket()
    {
        Packets = [];
    }

    public void AddPacket(Packet packet)
    {
        Packets.Add(packet);
    }

    public override string ToString()
    {
        return $"[{string.Join(',', Packets.Select(x => x.ToString()))}]";
    }

    public override int CompareTo(IntegerPacket integerPacket)
    {
        return CompareTo(integerPacket.ToListPacket());
    }

    public override int CompareTo(ListPacket listPacket)
    {
        for (var i = 0; i < Math.Min(Packets.Count, listPacket.Packets.Count); i++)
        {
            var result = Packets[i].CompareTo(listPacket.Packets[i]);
            if (result != 0)
            {
                return result;
            }
        }

        return Packets.Count.CompareTo(listPacket.Packets.Count);
    }
}
