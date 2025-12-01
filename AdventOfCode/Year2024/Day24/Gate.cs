namespace AdventOfCode.Year2024.Day24;

internal class Gate
{
    private bool? _firstValue;

    public Wire OutputWire { get; }

    public GateType GateType { get; }

    public Gate(GateType gateType, Wire outputWire)
    {
        GateType = gateType;
        OutputWire = outputWire;
    }

    public void SendValue(bool value)
    {
        if (!_firstValue.HasValue)
        {
            _firstValue = value;
            return;
        }

        var result = GateType switch
        {
            GateType.And => _firstValue.Value && value,
            GateType.Or => _firstValue.Value || value,
            GateType.Xor => _firstValue.Value ^ value,
            _ => throw new NotImplementedException()
        };

        _firstValue = null;

        OutputWire.SendValue(result);
    }
}
