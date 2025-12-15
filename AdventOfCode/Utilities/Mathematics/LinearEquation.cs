using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Utilities.Mathematics;

internal class LinearEquation
{
    public int Variable { get; }

    public LinearExpression Expression { get; }

    public LinearEquation(int variable)
    {
        Variable = variable;
        Expression = new LinearExpression(
        [
            new LinearTerm
            {
                Coefficient = RationalNumber.One,
                Variable = variable
            }
        ]);
    }

    public LinearEquation(int variable, LinearExpression expression)
    {
        Variable = variable;
        Expression = expression;
    }

    public override string ToString()
    {
        return $"x{Variable}={Expression}";
    }
}
