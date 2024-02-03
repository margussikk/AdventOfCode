namespace AdventOfCode.Year2020.Day05;

internal class BoardingPass
{
    public string Seat { get; private set; } = string.Empty;

    public int SeatRow { get; private set; }

    public int SeatColumn { get; private set; }

    public int SeatId => SeatRow * 8 + SeatColumn;

    public static BoardingPass Parse(string input)
    {
        var boardingPass = new BoardingPass
        {
            Seat = input,
            SeatRow = 0,
            SeatColumn = 0
        };

        foreach (var character in input.Take(7))
        {
            boardingPass.SeatRow *= 2;
            if (character == 'B')
            {
                boardingPass.SeatRow++;
            }
        }

        foreach (var character in input.Skip(3))
        {
            boardingPass.SeatColumn *= 2;
            if (character == 'R')
            {
                boardingPass.SeatColumn++;
            }
        }

        return boardingPass;
    }
}
