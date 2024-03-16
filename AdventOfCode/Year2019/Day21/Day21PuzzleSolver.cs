using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Year2019.IntCode;
using Microsoft.Diagnostics.Runtime.Utilities;
using System.Text;

namespace AdventOfCode.Year2019.Day21;

[Puzzle(2019, 21, "Springdroid Adventure")]
public class Day21PuzzleSolver : IPuzzleSolver
{
    private IntCodeProgram _program = new();

    public void ParseInput(string[] inputLines)
    {
        _program = IntCodeProgram.Parse(inputLines[0]);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var computer = new IntCodeComputer();
        computer.Load(_program);

        var stringBuilder = new StringBuilder();
        stringBuilder.Append("OR A J\n");  // J = A is ground
        stringBuilder.Append("AND B J\n"); // J = A and B is ground
        stringBuilder.Append("AND C J\n"); // J = A, B and C are ground
        stringBuilder.Append("NOT J J\n"); // J = If A, B and C are all ground then do not jump
        stringBuilder.Append("AND D J\n"); // J = D is ground and need to jump
        stringBuilder.Append("WALK\n");

        foreach (var character in stringBuilder.ToString())
        {
            computer.Inputs.Enqueue(Convert.ToInt64(character));
        }

        computer.Run();

        var answer = computer.Outputs.Last();      

        return new PuzzleAnswer(answer, 19361332);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var computer = new IntCodeComputer();
        computer.Load(_program);

        var stringBuilder = new StringBuilder();
        stringBuilder.Append("OR A J\n");  // J = A is ground
        stringBuilder.Append("AND B J\n"); // J = A and B is ground
        stringBuilder.Append("AND C J\n"); // J = A, B and C are ground
        stringBuilder.Append("NOT J J\n"); // J = If A, B and C are all ground then do not jump.
        stringBuilder.Append("AND D J\n"); // J = D is ground and need to jump

        stringBuilder.Append("OR E T\n");  // T = E is ground
        stringBuilder.Append("OR H T\n");  // T = E or H is ground

        stringBuilder.Append("AND T J\n"); // J = (A or B or C is a hole) and D is ground and (E or H is ground)
        stringBuilder.Append("RUN\n");


        //Shorter, doesn't use T
        //stringBuilder.Append("NOT H J\n");
        //stringBuilder.Append("OR C J\n");
        //stringBuilder.Append("AND B J\n");
        //stringBuilder.Append("AND A J\n");
        //stringBuilder.Append("NOT J J\n");
        //stringBuilder.Append("AND D J\n");
        //stringBuilder.Append("RUN\n");

        foreach (var character in stringBuilder.ToString())
        {
            computer.Inputs.Enqueue(Convert.ToInt64(character));
        }

        computer.Run();

        var answer = computer.Outputs.Last();

        return new PuzzleAnswer(answer, 1143351187L);
    }
}