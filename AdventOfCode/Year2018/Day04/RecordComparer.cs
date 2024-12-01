namespace AdventOfCode.Year2018.Day04;

internal class RecordComparer : IComparer<Record>
{
    public int Compare(Record? firstRecord, Record? secondRecord)
    {
        ArgumentNullException.ThrowIfNull(firstRecord);
        ArgumentNullException.ThrowIfNull(secondRecord);

        var compareResult = firstRecord.Month.CompareTo(secondRecord.Month);
        if (compareResult != 0)
        {
            return compareResult;
        }

        compareResult = firstRecord.Day.CompareTo(secondRecord.Day);
        if (compareResult != 0)
        {
            return compareResult;
        }

        compareResult = firstRecord.Hour.CompareTo(secondRecord.Hour);
        if (compareResult != 0)
        {
            return compareResult;
        }

        compareResult = firstRecord.Minute.CompareTo(secondRecord.Minute);
        if (compareResult != 0)
        {
            return compareResult;
        }

        return 0;
    }
}
