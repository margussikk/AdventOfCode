using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2019.Day03;

[Puzzle(2019, 3, "Crossed Wires")]
public class Day03PuzzleSolver : IPuzzleSolver
{
    private List<Instruction> _wire1Instructions = [];
    private List<Instruction> _wire2Instructions = [];

    public void ParseInput(string[] inputLines)
    {
        _wire1Instructions = inputLines[0].Split(',')
                                          .Select(Instruction.Parse)
                                          .ToList();

        _wire2Instructions = inputLines[1].Split(',')
                                          .Select(Instruction.Parse)
                                          .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        // Wires line segments
        var wire1LineSegments = GetLineSegments(_wire1Instructions);
        var wire2LineSegments = GetLineSegments(_wire2Instructions);

        // Answer
        var intersections = FindWireIntersections(wire1LineSegments, wire2LineSegments);

        var answer = intersections.Min(coordinate => Math.Abs(coordinate.X) + Math.Abs(coordinate.Y));

        return new PuzzleAnswer(answer, 1285);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        // Wires line segments
        var wire1LineSegments = GetLineSegments(_wire1Instructions);
        var wire2LineSegments = GetLineSegments(_wire2Instructions);

        // Answer
        var intersections = FindWireIntersections(wire1LineSegments, wire2LineSegments);

        var wire1Steps = CountSteps(wire1LineSegments, intersections);
        var wire2Steps = CountSteps(wire2LineSegments, intersections);

        var answer = intersections.Min(coordinate => wire1Steps[coordinate] + wire2Steps[coordinate]);

        return new PuzzleAnswer(answer, 14228);
    }

    private static List<LineSegment2D> GetLineSegments(List<Instruction> instructions)
    {
        var lineSegments = new List<LineSegment2D>();

        var currentGridCoordinate = new GridCoordinate(0, 0);
        foreach (var instruction in instructions)
        {
            var startCoordinate = new Coordinate2D(currentGridCoordinate);

            currentGridCoordinate = currentGridCoordinate.Move(instruction.Direction, instruction.Steps);

            var endCoordinate = new Coordinate2D(currentGridCoordinate);

            lineSegments.Add(new LineSegment2D(startCoordinate, endCoordinate));
        }

        return lineSegments;
    }

    private static List<Coordinate2D> FindWireIntersections(List<LineSegment2D> wire1LineSegments, List<LineSegment2D> wire2LineSegments)
    {
        var intersections = new HashSet<Coordinate2D>();

        foreach (var wire1LineSegment in wire1LineSegments)
        {
            foreach (var wire2LineSegment in wire2LineSegments)
            {
                if (wire1LineSegment.TryFindOverlap(wire2LineSegment, out var overlapLineSegment) &&
                    overlapLineSegment.Start != Coordinate2D.Zero)
                {
                    foreach(var coordinate in overlapLineSegment)
                    {
                        intersections.Add(coordinate);
                    }
                }
            }
        }

        return [.. intersections];
    }

    private static Dictionary<Coordinate2D, long> CountSteps(List<LineSegment2D> lineSegments, List<Coordinate2D> intersections)
    {
        var stepCounts = new Dictionary<Coordinate2D, long>();
        var steps = 0L;
        
        foreach (var lineSegment in lineSegments)
        {
            foreach (var coordinate in intersections)
            {
                if (lineSegment.Contains(coordinate) && !stepCounts.ContainsKey(coordinate))
                {
                    stepCounts[coordinate] = steps + MeasurementFunctions.ManhattanDistance(lineSegment.Start, coordinate);
                }
            }

            steps += MeasurementFunctions.ManhattanDistance(lineSegment.Start, lineSegment.End);
        }

        return stepCounts;
    }
}