using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Year2019.IntCode;
using AdventOfCode.Year2023.Day14;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Year2019.Day25;

[Puzzle(2019, 25, "Cryostasis")]
public class Day25PuzzleSolver : IPuzzleSolver
{
    private IntCodeProgram _program = new();

    public void ParseInput(string[] inputLines)
    {
        _program = IntCodeProgram.Parse(inputLines[0]);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = string.Empty;
        var rooms = new Dictionary<string, Room>();

        var computer = new IntCodeComputer();
        computer.Load(_program);

        var stack = new Stack<string>();
        var path = new Stack<string>();

        var pathToSecurityCheckpoint = new List<string>();
        var directionToPressureSensitiveFloor = string.Empty;

        var leaveItems = new List<string>()
        {
            "photons", "escape pod", "molten lava", "infinite loop", "giant electromagnet"
        };

        var takenItems = new List<string>();

        // Collect all items and trace path to security checkpoint
        while (true)
        {
            computer.Run();

            var room = Room.Parse(computer.GetAsciiOutput());

            // Room name and description
            if (rooms.TryAdd(room.Name, room)) // First time here
            {
                if (room.Name == "Security Checkpoint")
                {
                    pathToSecurityCheckpoint = path.Reverse().ToList();

                    directionToPressureSensitiveFloor = room.Doors.Find(x => x != FlipDirection(pathToSecurityCheckpoint[^1]));
                }
                else
                {
                    var entranceDirection = stack.Count > 0 ? stack.Peek() : string.Empty;

                    foreach (var doorDirection in room.Doors.Where(d => d != entranceDirection))
                    {
                        stack.Push(FlipDirection(doorDirection)); // For backtracking
                        stack.Push(doorDirection);
                    }
                }

                // Items
                foreach (var item in room.Items.Where(x => !leaveItems.Contains(x)))
                {
                    computer.AddAsciiInput($"take {item}\n");
                    computer.Run();
                    computer.Outputs.Clear(); // Expect it to take it

                    takenItems.Add(item);
                }
            }

            if (stack.Count == 0) // Back at the start
            {
                break;
            }

            var moveDirection = stack.Pop();

            if (path.Count > 0 && moveDirection == FlipDirection(path.Peek()))
            {
                path.Pop();
            }
            else
            {
                path.Push(moveDirection);
            }

            computer.AddAsciiInput($"{moveDirection}\n");
        }

        // Move to security checkpoint room
        foreach(var direction in pathToSecurityCheckpoint)
        {
            computer.AddAsciiInput($"{direction}\n");
        }
        computer.Run();
        computer.Outputs.Clear();


        // Go through all item combinations and try which works
        for (var pattern = 0; pattern < (1 << takenItems.Count); pattern++)
        {
            var droppedItems = new List<string>();
            for (var itemIndex = 0; itemIndex < takenItems.Count; itemIndex++)
            {
                var bitmask = 1 << itemIndex;
                if ((pattern & bitmask) != 0)
                {
                    // Keep item
                }
                else
                {
                    var item = takenItems[itemIndex];

                    computer.AddAsciiInput($"drop {item}\n");
                    computer.Run();
                    computer.Outputs.Clear(); // Expect it to drop

                    droppedItems.Add(item);
                }
            }

            // Go to Pressure-Sensitive Floor room
            computer.AddAsciiInput($"{directionToPressureSensitiveFloor}\n");
            computer.Run();

            var chunks = computer.GetAsciiOutput()
                                 .Split('\n')
                                 .SelectToChunks();

            if (chunks[2][0].Contains("you enter the cockpit"))
            {
                var textPattern = "typing ";

                var index = chunks[2][2].IndexOf(textPattern);
                index += textPattern.Length;

                answer = new string(chunks[2][2]
                    .Skip(index)
                    .TakeWhile(c => c != ' ')
                    .ToArray());

                break;
            }

            // Drop items
            foreach (var item in droppedItems)
            {
                computer.AddAsciiInput($"take {item}\n");
                computer.Run();
                computer.Outputs.Clear();
            }
        }

        return new PuzzleAnswer(answer, "20483");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        return new PuzzleAnswer("Merry Christmas", "Merry Christmas");
    }

    private static string FlipDirection(string direction)
    {
        return direction switch
        {
            "south" => "north",
            "north" => "south",
            "east" => "west",
            "west" => "east",
            _ => throw new InvalidOperationException($"Invalid direction {direction}")
        };
    }
}