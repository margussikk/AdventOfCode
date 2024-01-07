using AdventOfCode.Utilities.Graph;

namespace AdventOfCode.Year2023.Day25;

internal sealed record ComponentVisitor(GraphVertex Component, int[] WireIds);
