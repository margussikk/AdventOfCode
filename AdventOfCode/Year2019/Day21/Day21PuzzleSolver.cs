using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Year2019.IntCode;

namespace AdventOfCode.Year2019.Day21;

[Puzzle(2019, 21, "Springdroid Adventure")]
public class Day21PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<long> _program = [];

    public void ParseInput(string[] inputLines)
    {
        _program = inputLines[0].SelectToLongs(',');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var computer = new IntCodeComputer(_program);

        var inputs = new string[]
        {
            "OR A J",  // J = A is ground
            "AND B J", // J = A and B is ground
            "AND C J", // J = A, B and C are ground
            "NOT J J", // J = If A, B and C are all ground then do not jump
            "AND D J", // J = D is ground and need to jump
            "WALK"
        };

        var result = computer.Run(inputs);

        var answer = result.Outputs[^1];

        return new PuzzleAnswer(answer, 19361332);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var computer = new IntCodeComputer(_program);

        var inputs = new string[]
        {
            "OR A J",  // J = A is ground
            "AND B J", // J = A and B is ground
            "AND C J", // J = A, B and C are ground
            "NOT J J", // J = If A, B and C are all ground then do not jump.
            "AND D J", // J = D is ground and need to jump

            "OR E T",  // T = E is ground
            "OR H T",  // T = E or H is ground

            "AND T J", // J = (A or B or C is a hole) and D is ground and (E or H is ground)
            "RUN"
        };


        //Shorter and doesn't use T
        //var inputs = new string[]
        //{
        //    "NOT H J",
        //    "OR C J",
        //    "AND B J",
        //    "AND A J",
        //    "NOT J J",
        //    "AND D J",
        //    "RUN",
        //};

        var result = computer.Run(inputs);

        var answer = result.Outputs[^1];

        return new PuzzleAnswer(answer, 1143351187L);
    }
}