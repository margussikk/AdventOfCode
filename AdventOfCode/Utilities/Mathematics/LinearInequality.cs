using AdventOfCode.Utilities.Numerics;
using System.Text;

namespace AdventOfCode.Utilities.Mathematics;

internal class LinearInequality
{
    public int Variable { get; }
    public LinearExpression Expression { get; }
    public LinearInequalityConstraint Constraint { get; }

    public LinearInequality(LinearExpression expression, int variable, LinearInequalityConstraint constraint, RationalNumber constraintValue)
    {
        var newExpression = expression.Clone();

        var variableTerm = newExpression.ExtractVariable(variable);
        newExpression.Negate();
        newExpression.AddConstant(constraintValue);

        if (variableTerm.Coefficient < RationalNumber.Zero)
        {
            variableTerm = variableTerm.Negated();
            newExpression.Negate();

            constraint = constraint switch
            {
                LinearInequalityConstraint.GreaterThan => LinearInequalityConstraint.LessThan,
                LinearInequalityConstraint.GreaterThanOrEqual => LinearInequalityConstraint.LessThanOrEqual,
                LinearInequalityConstraint.LessThan => LinearInequalityConstraint.GreaterThan,
                LinearInequalityConstraint.LessThanOrEqual => LinearInequalityConstraint.GreaterThanOrEqual,
                _ => constraint,
            };
        }

        if (variableTerm.Coefficient != RationalNumber.One)
        {
            var divisor = variableTerm.Coefficient;

            variableTerm = variableTerm.DividedBy(divisor);
            newExpression.Divide(divisor);
        }

        Variable = variableTerm.Variable!.Value;
        Expression = newExpression;
        Constraint = constraint;
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append($"x{Variable}");

        var constraintString = Constraint switch
        {
            LinearInequalityConstraint.GreaterThan => ">",
            LinearInequalityConstraint.GreaterThanOrEqual => ">=",
            LinearInequalityConstraint.LessThan => "<",
            LinearInequalityConstraint.LessThanOrEqual => "<=",
            _ => string.Empty,
        };
        stringBuilder.Append(constraintString);
        stringBuilder.Append(Expression);
        
        return stringBuilder.ToString();
    }
}
