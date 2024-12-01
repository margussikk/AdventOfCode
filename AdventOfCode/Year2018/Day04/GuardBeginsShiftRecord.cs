namespace AdventOfCode.Year2018.Day04;

internal class GuardBeginsShiftRecord : Record
{
    public int GuardId { get; }

    public GuardBeginsShiftRecord(int month, int day, int hour, int minute, int guardId) : base(month, day, hour, minute)
    {
        GuardId = guardId;
    }

    public override string ToString()
    {
        return $"[1518-{Month:D2}-{Day:D2} {Hour:D2}:{Minute:D2}] Guard #{GuardId} begins shift";
    }
}
