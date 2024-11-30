using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2018.Day04;

[Puzzle(2018, 4, "Repose Record")]
public class Day04PuzzleSolver : IPuzzleSolver
{
    private List<Record> _records = [];

    public void ParseInput(string[] inputLines)
    {
        _records = inputLines.Select(Record.Parse)
                             .ToList();

        _records.Sort(new RecordComparer());
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var guardsSleepingMinutes = GetGuardsSleepingMinutes();

        var sleepiestGuardId = guardsSleepingMinutes.MaxBy(x => x.Value.Sum()).Key;

        var sleepiestMinute = guardsSleepingMinutes[sleepiestGuardId]
            .Select((timesSleeping, minute) => new
            {
                Minute = minute,
                TimesSleeping = timesSleeping,
            })
            .MaxBy(x => x.TimesSleeping)
            !.Minute;

        var answer = sleepiestGuardId * sleepiestMinute;

        return new PuzzleAnswer(answer, 50558);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var guardsSleepingMinutes = GetGuardsSleepingMinutes();

        var sleepiestGuardId = guardsSleepingMinutes.MaxBy(x => x.Value.Max()).Key;

        var sleepiestMinute = guardsSleepingMinutes[sleepiestGuardId]
            .Select((timesSleeping, minute) => new
            {
                Minute = minute,
                TimesSleeping = timesSleeping,
            })
            .MaxBy(x => x.TimesSleeping)
            !.Minute;

        var answer = sleepiestGuardId * sleepiestMinute;

        return new PuzzleAnswer(answer, 28198);
    }

    private Dictionary<int, int[]> GetGuardsSleepingMinutes()
    {
        var guardsSleepingMinutes = new Dictionary<int, int[]>();

        var currentGuardId = 0;
        var currentGuardFellAsleepHour = 0;
        var currentGuardFellAsleepMinute = 0;
        var isCurrentGuardSleeping = false;

        foreach (var record in _records)
        {
            if (record is GuardBeginsShiftRecord guardBeginsShiftRecord)
            {
                if (isCurrentGuardSleeping)
                {
                    throw new InvalidOperationException("Current guard is sleeping");
                }

                currentGuardId = guardBeginsShiftRecord.GuardId;

                if (!guardsSleepingMinutes.ContainsKey(currentGuardId))
                {
                    guardsSleepingMinutes[currentGuardId] = new int[60];
                }

                isCurrentGuardSleeping = false;
            }
            else if (record is GuardFallsAsleepRecord guardFallsAsleepRecord)
            {
                if (isCurrentGuardSleeping)
                {
                    throw new InvalidOperationException("Guard is already sleeping");
                }

                currentGuardFellAsleepHour = guardFallsAsleepRecord.Hour;
                currentGuardFellAsleepMinute = guardFallsAsleepRecord.Minute;
                isCurrentGuardSleeping = true;
            }
            else if (record is GuardWakesUpRecord guardWakesUpRecord)
            {
                if (!isCurrentGuardSleeping)
                {
                    throw new InvalidOperationException("Guard has to be sleeping");
                }

                if (currentGuardFellAsleepHour != 0)
                {
                    throw new NotImplementedException("Guard fell asleep before midnight");
                }

                if (!guardsSleepingMinutes.TryGetValue(currentGuardId, out var sleepMinutes))
                {
                    throw new InvalidOperationException("Couldn't get current guard sleep minutes");
                }

                for (var minute = currentGuardFellAsleepMinute; minute < guardWakesUpRecord.Minute; minute++)
                {
                    sleepMinutes[minute]++;
                }

                isCurrentGuardSleeping = false;
            }
        }

        if (isCurrentGuardSleeping)
        {
            throw new InvalidOperationException("Guard is still sleeping");
        }

        return guardsSleepingMinutes;
    }
}