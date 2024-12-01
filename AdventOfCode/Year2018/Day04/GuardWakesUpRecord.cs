namespace AdventOfCode.Year2018.Day04;

internal class GuardWakesUpRecord : Record
{
    public GuardWakesUpRecord(int month, int day, int hour, int minute) : base(month, day, hour, minute) { }

    public override string ToString()
    {
        return $"[1518-{Month:D2}-{Day:D2} {Hour:D2}:{Minute:D2}] wakes up";
    }
}
