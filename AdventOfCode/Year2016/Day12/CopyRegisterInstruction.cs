namespace AdventOfCode.Year2016.Day12;

internal class CopyRegisterInstruction : Instruction
{
    public int SourceRegister { get; }
    public int DestinationRegister { get; }
    
    public CopyRegisterInstruction(int sourceRegister, int destinationRegister)
    {
        SourceRegister = sourceRegister;
        DestinationRegister = destinationRegister;
    }

    public override void Execute(int[] registers, ref int instructionPointer)
    {
        registers[DestinationRegister] = registers[SourceRegister];
        instructionPointer++;
    }
}
