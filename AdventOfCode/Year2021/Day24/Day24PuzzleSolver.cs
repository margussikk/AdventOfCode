using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2021.Day24;

/* There are 14 groups of instructions, which differ only at 3 places. 7 groups increase z value, 7 decrease z value. z gets its next value from formula
    "z = 26 * z + Input + Addition". The goal is to get z to 0. If z is 1 in the divide instruction then its next instruction (add x 13) has input difference
    over 10 which makes the input comparisons fail. However it is possible to decrease z when the divider is 26 by choosing an input which satisfies the condition
    "input == (z % 26) + InputDifference". Basically the instructions multiply z by 26 and divide by 26 and if all the inputs are correct then it divides back to 0.

    inp w
    mul x 0
    add x z
    mod x 26
    div z 1  <--- divider, it determines if z gets bigger or smaller
    add x 13 <--- input difference
    eql x w
    eql x 0
    mul y 0
    add y 25
    mul y x
    add y 1
    mul z y
    mul y 0
    add y w
    add y 6 <--- addition
    mul y x
    add z y
*/

[Puzzle(2021, 24, "Arithmetic Logic Unit")]
public class Day24PuzzleSolver : IPuzzleSolver
{
    private List<Instruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        _instructions = inputLines.Select(Instruction.Parse)
                                  .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var (_, maxModelNumbers) = GetMinMaxModelNumbers();

        var answer = maxModelNumbers.Aggregate(0L, (acc, next) => acc * 10 + next);

        return new PuzzleAnswer(answer, 94992992796199L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var (minModelNumbers, _) = GetMinMaxModelNumbers();

        var answer = minModelNumbers.Aggregate(0L, (acc, next) => acc * 10 + next);

        return new PuzzleAnswer(answer, 11931881141161L);
    }

    private void Verify()
    {
        var (minModelNumber, maxModelNumber) = GetMinMaxModelNumbers();

        var alu = new Alu();

        alu.Execute(minModelNumber, _instructions);
        Console.WriteLine($"min valid={(alu.Variables['z' - 'w'] == 0)}");

        alu.Execute(maxModelNumber, _instructions);
        Console.WriteLine($"max valid={(alu.Variables['z' - 'w'] == 0)}");
    }

    private (int[] minModelNumber, int[] maxModelNumber) GetMinMaxModelNumbers()
    {
        var minModelNumber = new int[14];
        var maxModelNumber = new int[14];

        var stack = new Stack<(int InputIndex, int Addition)>();

        var instructionGroupData = GetInstructionGroupData(_instructions);
        for (var currentInputIndex = 0; currentInputIndex < instructionGroupData.Count; currentInputIndex++)
        {
            if (instructionGroupData[currentInputIndex].Divider == 1)
            {
                // z stays the same or increases
                stack.Push((currentInputIndex, instructionGroupData[currentInputIndex].Addition));
            }
            else
            {
                // z decreases
                var (otherInputIndex, addition) = stack.Pop();
                var difference = instructionGroupData[currentInputIndex].InputDifference + addition;

                var (currentInputMax, otherInputMax) = GetInputValues(9, difference);
                maxModelNumber[currentInputIndex] = currentInputMax;
                maxModelNumber[otherInputIndex] = otherInputMax;

                var (currentInputMin, otherInputMin) = GetInputValues(1, difference);
                minModelNumber[currentInputIndex] = currentInputMin;
                minModelNumber[otherInputIndex] = otherInputMin;
            }
        }

        return (minModelNumber, maxModelNumber);

        // Local methods
        static (int currentInputValue, int otherInputValue) GetInputValues(int otherInputValue, int difference)
        {
            var currentInputValue = otherInputValue + difference;
            if (currentInputValue <= 0)
            {
                currentInputValue = 1;
            }
            else if (currentInputValue > 9)
            {
                currentInputValue = 9;
            }

            otherInputValue = currentInputValue - difference;

            return (currentInputValue, otherInputValue);
        }
    }

    private static List<InstructionGroupData> GetInstructionGroupData(List<Instruction> instructions)
    {
        var instructionCounter = 0;
        var instructionGroups = new List<InstructionGroupData>();
        InstructionGroupData? instructionGroup = null;

        foreach (var instruction in instructions)
        {
            if (instruction.Code == InstructionCode.Inp)
            {
                if (instructionGroup != null)
                {
                    instructionGroups.Add(instructionGroup);
                }

                instructionGroup = new InstructionGroupData();
                instructionCounter = 0;
            }
            else
            {
                if (instructionGroup == null)
                {
                    throw new InvalidOperationException();
                }

                if (instructionCounter == 4 && instruction.Code == InstructionCode.Div && instruction.ParameterB is NumberParameter numberParameterB1)
                {
                    instructionGroup.Divider = numberParameterB1.Number;
                }
                else if (instructionCounter == 5 && instruction.Code == InstructionCode.Add && instruction.ParameterB is NumberParameter numberParameterB2)
                {
                    instructionGroup.InputDifference = numberParameterB2.Number;
                }
                else if (instructionCounter == 15 && instruction.Code == InstructionCode.Add && instruction.ParameterB is NumberParameter numberParameterB3)
                {
                    instructionGroup.Addition = numberParameterB3.Number;
                }
            }

            instructionCounter++;
        }

        if (instructionGroup != null)
        {
            instructionGroups.Add(instructionGroup);
        }

        return instructionGroups;
    }
}