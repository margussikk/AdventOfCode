namespace AdventOfCode.Year2018.Day09
{
    internal class Marble
    {
        public long Worth { get; }

        public Marble ClockwiseMarble { get; set; }

        public Marble CounterClockwiseMarble { get; set; }

        public Marble(long worth)
        {
            Worth = worth;

            CounterClockwiseMarble = this;
            ClockwiseMarble = this;            
        }

        public void AddClockwise(Marble marble)
        {
            var before = this;
            var after = ClockwiseMarble;

            marble.CounterClockwiseMarble = before;
            marble.ClockwiseMarble = after;

            before.ClockwiseMarble = marble;
            after.CounterClockwiseMarble = marble;
        }

        public void Remove()
        {
            var before = CounterClockwiseMarble;
            var after = ClockwiseMarble;

            before.ClockwiseMarble = after;
            after.CounterClockwiseMarble = before;
        }
    }
}
