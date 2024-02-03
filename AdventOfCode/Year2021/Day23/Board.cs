using System;
using System.Numerics;
using System.Text;

namespace AdventOfCode.Year2021.Day23;

internal class Board
{
    public State[] Hallway { get; private set; } = new State[11];

    public State[][] Rooms { get; private set; } = new State[4][];

    public int Energy { get; set; }

    public Board? PreviousBoard { get; set; }

    public Board()
    {
        Array.Fill(Hallway, State.Empty);

        for (var i = 0; i < Rooms.Length; i++)
        {
            Rooms[i] = new State[2];
            Array.Fill(Rooms[i], State.Empty);
        }
    }

    public int GetStatesHashCode()
    {
        var hashCode = new HashCode();

        foreach (var hallway in Hallway)
        {
            hashCode.Add(hallway.GetHashCode());
        }

        foreach (var room in Rooms)
        {
            foreach (var roomSlot in room)
            {
                hashCode.Add(roomSlot.GetHashCode());
            }
        }

        return hashCode.ToHashCode();
    }

    public bool IsSolved()
    {
        for (var i = 0; i < Rooms.Length; i++)
        {
            if (Array.Exists(Rooms[i], x => (int)x != i))
            {
                return false;
            }
        }

        return true;
    }

    public static Board Parse(string[] lines)
    {
        var board = new Board();

        // Assume a specific format
        if (lines[0] != "#############" || !lines[^1].StartsWith("  #########"))
        {
            throw new InvalidOperationException("Incorrect board input");
        }

        // Hallway
        for (var hallwaySlot = 0; hallwaySlot < board.Hallway.Length; hallwaySlot++)
        {
            if (lines[1][hallwaySlot + 1] == '.')
            {
                board.Hallway[hallwaySlot] = State.Empty;
            }
            else
            {
                board.Hallway[hallwaySlot] = Enum.Parse<State>(lines[1][hallwaySlot + 1].ToString());
            }
        }

        // Rooms
        for (var roomIndex = 0; roomIndex < board.Rooms.Length; roomIndex++)
        {
            board.Rooms[roomIndex] = new State[lines.Length - 3];
        }

        for (var roomSlot = 0; roomSlot < board.Rooms[0].Length; roomSlot++)
        {
            for (var roomIndex = 0; roomIndex < board.Rooms.Length; roomIndex++)
            {
                if (lines[roomSlot + 2][roomIndex * 2 + 3] == '.')
                {
                    board.Rooms[roomIndex][roomSlot] = State.Empty;
                }
                else
                {
                    board.Rooms[roomIndex][roomSlot] = Enum.Parse<State>(lines[roomSlot + 2][roomIndex * 2 + 3].ToString());
                }
            }
        }

        return board;
    }

    public List<Board> GetNextBoards()
    {
        var boards = new List<Board>();

        // Move from hallway to room
        for (var hallwaySlot = 0; hallwaySlot < Hallway.Length; hallwaySlot++)
        {
            var amphipod = Hallway[hallwaySlot];
            if (amphipod == State.Empty)
            {
                // No Amphipod, no problem
                continue;
            }

            int roomIndex = (int)amphipod;

            // Verify that destination does not contain strangers
            var shouldMoveToRoom = ShouldMoveFromHallwayToRoom(amphipod, roomIndex);
            if (!shouldMoveToRoom)
            {
                // Destination has strangers
                continue;
            }

            if (CanMoveBetweenRoomAndHallway(roomIndex, hallwaySlot, true))
            {
                var nextBoard = Copy();
                var roomSlot = Array.LastIndexOf(nextBoard.Rooms[roomIndex], State.Empty);

                nextBoard.Energy = Energy + CalculateEnergy(amphipod, roomIndex, roomSlot, hallwaySlot);
                nextBoard.Rooms[roomIndex][roomSlot] = amphipod;
                nextBoard.Hallway[hallwaySlot] = State.Empty;

                boards.Add(nextBoard);
            }
        }

        // Move from room to hallway
        for (var roomSlot = 0; roomSlot < Rooms[0].Length; roomSlot++)
        {
            for (var roomIndex = 0; roomIndex < Rooms.Length; roomIndex++)
            {
                var amphipod = Rooms[roomIndex][roomSlot];
                if (amphipod == State.Empty)
                {
                    // No Amphipod, no problem
                    continue;
                }

                var shouldMoveToHallway = ShouldMoveFromRoomToHallway(amphipod, roomIndex);
                if (!shouldMoveToHallway)
                {
                    // Amphipod is at destination
                    continue;
                }

                var canMoveOutOfRoom = Rooms[roomIndex].Take(roomSlot).All(x => x == State.Empty);
                if (!canMoveOutOfRoom)
                {
                    // Someone is blocking in the room (i.e. someone is above)
                    continue;
                }

                var entrances = new int[] { 2, 4, 6, 8 };
                for (var hallwaySlot = 0; hallwaySlot < Hallway.Length; hallwaySlot++)
                {
                    if (entrances.Contains(hallwaySlot))
                    {
                        // Can't move to entrance
                        continue;
                    }

                    if (CanMoveBetweenRoomAndHallway(roomIndex, hallwaySlot, false))
                    {
                        var newBoard = Copy();

                        newBoard.Energy = Energy + CalculateEnergy(amphipod, roomIndex, roomSlot, hallwaySlot);
                        newBoard.Rooms[roomIndex][roomSlot] = State.Empty;
                        newBoard.Hallway[hallwaySlot] = amphipod;

                        boards.Add(newBoard);
                    }
                }
            }
        }

        return boards;
    }

    public Board Copy()
    {
        var board = new Board
        {
            PreviousBoard = this,
            Hallway = new State[Hallway.Length],
            Rooms = new State[Rooms.Length][]
        };

        Array.Copy(Hallway, board.Hallway, Hallway.Length);

        for (var i = 0; i < Rooms.Length; i++)
        {
            board.Rooms[i] = new State[Rooms[i].Length];
            Array.Copy(Rooms[i], board.Rooms[i], Rooms[i].Length);
        }

        return board;
    }

    private bool ShouldMoveFromHallwayToRoom(State amphipod, int roomIndex)
    {
        if (roomIndex != (int)amphipod)
        {
            // Amphipod cannot move to that room
            return false;
        }

        return Array.TrueForAll(Rooms[(int)amphipod], x => x == State.Empty || x == amphipod);
    }

    private bool ShouldMoveFromRoomToHallway(State amphipod, int roomIndex)
    {
        if (roomIndex != (int)amphipod)
        {
            // Amphipod is in the wrong room and should move out
            return true;
        }

        // Amphipod is in the correct room, check if this room contains strangers
        return Array.Exists(Rooms[roomIndex], x => x != amphipod && x != State.Empty);
    }

    private bool CanMoveBetweenRoomAndHallway(int roomIndex, int hallwaySlot, bool fromHallwayToRoom)
    {
        var entrance = 2 + roomIndex * 2;

        if (hallwaySlot < entrance)
        {
            return Hallway.Skip(hallwaySlot + (fromHallwayToRoom ? 1 : 0)).Take(entrance - hallwaySlot).All(x => x == State.Empty);
        }
        else
        {
            return Hallway.Skip(entrance + (fromHallwayToRoom ? 0 : 1)).Take(hallwaySlot - entrance).All(x => x == State.Empty);
        }
    }

    private int CalculateEnergy(State amphipod, int roomIndex, int roomSlot, int hallwaySlot)
    {
        var stepEnergy = Convert.ToInt32(Math.Pow(10, (int)amphipod));

        // Energy to move in the room + entrance
        var totalEnergy = stepEnergy * (Rooms[roomIndex].Take(roomSlot).Count() + 1);

        // Energy to move in the hallway
        var entrance = 2 + roomIndex * 2;
        totalEnergy += stepEnergy * Math.Abs(entrance - hallwaySlot);

        return totalEnergy;
    }

    public void Print()
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("#############");

        // Hallway
        stringBuilder.Append('#');
        foreach (var hallwaySlot in Hallway)
        {
            stringBuilder.Append(StateSymbol(hallwaySlot));
        }
        stringBuilder.AppendLine("#");

        // Rooms
        for (var roomSlot = 0; roomSlot < Rooms[0].Length; roomSlot++)
        {
            if (roomSlot == 0)
            {
                stringBuilder.Append("##");
            }
            else
            {
                stringBuilder.Append("  ");
            }

            stringBuilder.Append($"#{StateSymbol(Rooms[0][roomSlot])}#{StateSymbol(Rooms[1][roomSlot])}#{StateSymbol(Rooms[2][roomSlot])}#{StateSymbol(Rooms[3][roomSlot])}#");

            if (roomSlot == 0)
            {
                stringBuilder.Append("##");
            }
            else
            {
                stringBuilder.Append("  ");
            }

            stringBuilder.AppendLine();
        }
        stringBuilder.AppendLine("  #########");
        stringBuilder.AppendLine($"Energy = {Energy}");

        Console.WriteLine(stringBuilder.ToString());

        // Local
        static string StateSymbol(State space)
        {
            if (space == State.Empty)
            {
                return ".";
            }
            else
            {
                return space.ToString();
            }
        }
    }

    public void PrintSteps()
    {
        var boards = new List<Board>();

        var currentBoard = this;
        while (currentBoard != null)
        {
            boards.Add(currentBoard);
            currentBoard = currentBoard.PreviousBoard;
        }

        boards.Reverse();

        foreach (var board in boards)
        {
            board.Print();
        }
    }
}
