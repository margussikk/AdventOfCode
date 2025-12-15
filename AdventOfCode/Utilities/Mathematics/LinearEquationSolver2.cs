using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Utilities.Mathematics;

internal static class LinearEquationSolver2
{
    public static IEnumerable<RationalNumber[]> Solve(IReadOnlyList<LinearEquation> equations, long minVariableValue, long maxVariableValue)
    {
        var freeVariableExpressions = equations
            .SelectMany(e => e.Expression.Terms.Where(x => x.Variable.HasValue).Select(x => x.Variable!.Value))
            .GroupBy(x => x)
            .Select(x => (Variable: x.Key, Count: x.Count()))
            .OrderByDescending(x => x.Count)
            .Select(x =>
            {
                var expressions = equations
                    .Where(e => e.Expression.ContainsVariable(x.Variable))
                    .Select(e => e.Expression)
                    .ToArray();
                return new KeyValuePair<int, LinearExpression[]>(x.Variable, expressions);
            })
            .ToArray();

        var initialVariableValues = equations
            .Select(x => x.Expression.IsConstantExpression() ? x.Expression.GetConstant() : RationalNumber.Zero)
            .ToArray();

        return FindSolutions(equations, freeVariableExpressions, [], initialVariableValues, minVariableValue, maxVariableValue);
    }

    private static IEnumerable<RationalNumber[]> FindSolutions(IReadOnlyList<LinearEquation> equations, KeyValuePair<int, LinearExpression[]>[] freeVariableExpressions, KeyValuePair<int, RationalNumber>[] freeVariableValues, RationalNumber[] initialVariableValues, long minVariableValue, long maxVariableValue)
    {
        if (freeVariableValues.Length == freeVariableExpressions.Length)
        {
            var variableValues = initialVariableValues.ToArray();

            foreach (var freeVariableValue in freeVariableValues)
            {
                variableValues[freeVariableValue.Key] = freeVariableValue.Value;
            }

            foreach (var equation in equations)
            {
                variableValues[equation.Variable] = equation.Expression.Evaluate(variableValues);
            }

            yield return variableValues;
            yield break;
        }

        // Substitute free variables with selected values
        var modifiedExpressions = freeVariableExpressions[freeVariableValues.Length].Value
            .Select(x => x.Clone())
            .ToList();

        foreach (var expression in modifiedExpressions)
        {
            foreach(var freeVariableValue in freeVariableValues)
            {
                expression.SetVariableValue(freeVariableValue.Key, freeVariableValue.Value);
            }
        }

        var lowerBound = minVariableValue;
        var lowerBoundRationalNumber = new RationalNumber(lowerBound);
        
        var upperBound = maxVariableValue;
        var upperBoundRationalNumber = new RationalNumber(upperBound);

        var currentVariable = freeVariableExpressions[freeVariableValues.Length].Key;

        var boundExpressions = modifiedExpressions
            .Select(e => new LinearInequality(e, currentVariable, LinearInequalityConstraint.GreaterThanOrEqual, lowerBoundRationalNumber))
            .Concat(modifiedExpressions.Select(e => new LinearInequality(e, currentVariable, LinearInequalityConstraint.LessThanOrEqual, upperBoundRationalNumber)))
            .ToList();

        foreach (var inequality in boundExpressions)
        {
            if (inequality.Expression.IsConstantExpression())
            {
                var constant = inequality.Expression.GetConstant();
                var value = constant.LongValue;

                if (inequality.Constraint == LinearInequalityConstraint.GreaterThanOrEqual)
                {
                    if (!constant.IsWholeNumber && constant > RationalNumber.Zero)
                    {
                        value++;
                    }

                    lowerBound = Math.Max(lowerBound, value);
                }
                else if (inequality.Constraint == LinearInequalityConstraint.LessThanOrEqual)
                {
                    if (!constant.IsWholeNumber && constant < RationalNumber.Zero)
                    {
                        value--;
                    }

                    upperBound = Math.Min(upperBound, value);
                }
            }
        }

        for (var value = lowerBound; value <= upperBound; value++)
        {
            KeyValuePair<int, RationalNumber>[] nextFreeVariableValues = [..freeVariableValues, new KeyValuePair<int, RationalNumber>(currentVariable, new RationalNumber(value))];

            foreach (var solution in FindSolutions(equations, freeVariableExpressions, nextFreeVariableValues, initialVariableValues, minVariableValue, maxVariableValue))
            {
                yield return solution;
            }
        }
    }
}
