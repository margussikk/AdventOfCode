namespace AdventOfCode.Year2021.Day16;

internal class Packet
{
    public int Version { get; private set; }
    public PacketType Type { get; private set; }
    public List<Packet> Packets { get; } = [];

    public long Value { get; private set; }

    public static Packet Build(BitReader bitReader)
    {
        var packet = new Packet
        {
            Version = bitReader.Read(3),
            Type = (PacketType)bitReader.Read(3)
        };

        if (packet.Type == PacketType.LiteralValue)
        {
            long group;
            do
            {
                group = bitReader.Read(5);

                packet.Value <<= 4;
                packet.Value |= (group & 0b1111);
            }
            while ((group & 0b1_0000) == 0b1_0000);
        }
        else
        {
            var lengthTypeId = bitReader.Read(1);
            if (lengthTypeId == 0)
            {
                var totalLength = bitReader.Read(15);
                var startBitIndex = bitReader.BitIndex;

                while (bitReader.BitIndex < startBitIndex + totalLength)
                {
                    var subPacket = Build(bitReader);
                    packet.Packets.Add(subPacket);
                }
            }
            else
            {
                var subPacketCount = bitReader.Read(11);
                for (var i = 0; i < subPacketCount; i++)
                {
                    var subPacket = Build(bitReader);
                    packet.Packets.Add(subPacket);
                }
            }
        }

        return packet;
    }

    public int SumVersions()
    {
        return Packets.Aggregate(Version, (a, b) => a + b.SumVersions());
    }

    public long Evaluate()
    {
        return Type switch
        {
            PacketType.Sum => Packets.Sum(x => x.Evaluate()),
            PacketType.Product => Packets.Aggregate(1L, (acc, current) => acc * current.Evaluate()),
            PacketType.Minimum => Packets.Min(x => x.Evaluate()),
            PacketType.Maximum => Packets.Max(x => x.Evaluate()),
            PacketType.LiteralValue => Value,
            PacketType.GreaterThan => Packets[0].Evaluate() > Packets[1].Evaluate() ? 1 : 0,
            PacketType.LessThan => Packets[0].Evaluate() < Packets[1].Evaluate() ? 1 : 0,
            PacketType.EqualTo => Packets[0].Evaluate() == Packets[1].Evaluate() ? 1 : 0,
            _ => 0,
        };
    }
}
