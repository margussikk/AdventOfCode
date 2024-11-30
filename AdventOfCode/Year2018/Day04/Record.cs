using System.Text.RegularExpressions;

namespace AdventOfCode.Year2018.Day04
{
    internal abstract partial class Record
    {
        public int Month { get; }

        public int Day { get; }

        public int Hour { get; }

        public int Minute { get; }

        protected Record(int month, int day, int hour, int minute)
        {
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
        }

        public static Record Parse(string input)
        {
            if (input.Contains("begins shift"))
            {
                var matches = GuardBeginsShiftInputLineRegex().Matches(input);
                if (matches.Count != 1)
                {
                    throw new InvalidOperationException("Failed to parse input line");
                }

                var match = matches[0];

                var month = int.Parse(match.Groups[1].Value);
                var day = int.Parse(match.Groups[2].Value);
                var hour = int.Parse(match.Groups[3].Value);
                var minute = int.Parse(match.Groups[4].Value);
                var guardId = int.Parse(match.Groups[5].Value);

                return new GuardBeginsShiftRecord(month, day, hour, minute, guardId);
            }
            else if (input.Contains("falls asleep"))
            {
                var matches = GuardFallsAsleepInputLineRegex().Matches(input);
                if (matches.Count != 1)
                {
                    throw new InvalidOperationException("Failed to parse input line");
                }

                var match = matches[0];

                var month = int.Parse(match.Groups[1].Value);
                var day = int.Parse(match.Groups[2].Value);
                var hour = int.Parse(match.Groups[3].Value);
                var minute = int.Parse(match.Groups[4].Value);

                return new GuardFallsAsleepRecord(month, day, hour, minute);
            }
            else if (input.Contains("wakes up"))
            {
                var matches = GuardWakesUpInputLineRegex().Matches(input);
                if (matches.Count != 1)
                {
                    throw new InvalidOperationException("Failed to parse input line");
                }

                var match = matches[0];

                var month = int.Parse(match.Groups[1].Value);
                var day = int.Parse(match.Groups[2].Value);
                var hour = int.Parse(match.Groups[3].Value);
                var minute = int.Parse(match.Groups[4].Value);

                return new GuardWakesUpRecord(month, day, hour, minute);
            }

            throw new NotImplementedException("Invalid input line");
        }

        [GeneratedRegex(@"\[1518-(\d+)-(\d+) (\d+):(\d+)\] Guard #(\d+) begins shift")]
        private static partial Regex GuardBeginsShiftInputLineRegex();

        [GeneratedRegex(@"\[1518-(\d+)-(\d+) (\d+):(\d+)\] falls asleep")]
        private static partial Regex GuardFallsAsleepInputLineRegex();

        [GeneratedRegex(@"\[1518-(\d+)-(\d+) (\d+):(\d+)\] wakes up")]
        private static partial Regex GuardWakesUpInputLineRegex();
    }
}
