using BenchmarkDotNet.Attributes;

namespace AdventOfCode.Framework.Puzzle;

[MemoryDiagnoser(displayGenColumns: false)]
public class PuzzleSolverBenchmarker<T> where T : IPuzzleSolver, new()
{
    private string[] _inputLines = [];

    [GlobalSetup]
    public void Setup()
    {
        _inputLines = PuzzleInputProvider.Instance.GetInputLines(typeof(T));
    }

    [Benchmark]
    public void ParseAndSolvePartOne()
    {
        T solver = new();

        solver.ParseInput(_inputLines);
        solver.GetPartOneAnswer();
    }

    [Benchmark]
    public void ParseAndSolvePartTwo()
    {
        T solver = new();

        solver.ParseInput(_inputLines);
        solver.GetPartTwoAnswer();
    }
}
