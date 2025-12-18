using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Utilities.Mathematics;

internal static class LinearEquationSolver
{


    public static RationalNumber[][] Solve(IReadOnlyList<LinearEquation> equations)
    {
        var variableValueLimits = Enumerable.Range(0, equations.Count)
                                            .Select(x => new NumberRange<long>(0, 0))
                                            .ToArray();

        return [.. Solve(equations, variableValueLimits)];
    }

    public static RationalNumber[][] Solve(IReadOnlyList<LinearEquation> equations, NumberRange<long>[] variableValueLimits)
    {
        var freeVariableEquationIndexes = equations
            .SelectMany(e => e.Expression.Terms.Where(x => x.Variable.HasValue).Select(x => x.Variable!.Value))
            .GroupBy(x => x)
            .Select(x => (Variable: x.Key, Count: x.Count()))
            .OrderByDescending(x => x.Count)
            .Select(x =>
            {
                var equationIndexes = Enumerable
                    .Range(0, equations.Count)
                    .Where(i => equations[i].Expression.ContainsVariable(x.Variable))
                    .ToArray();

                return new KeyValuePair<int, int[]>(x.Variable, equationIndexes);
            })
            .ToArray();

        var constantVariableValues = equations
            .Where(x => x.Expression.IsConstantExpression())
            .Select(x => new KeyValuePair<int, RationalNumber>(x.Variable, x.Expression.GetConstant()))
            .ToArray();

        return [.. FindSolutions(equations, freeVariableEquationIndexes, [], constantVariableValues, variableValueLimits)];
    }

    private static IEnumerable<RationalNumber[]> FindSolutions(IReadOnlyList<LinearEquation> equations, KeyValuePair<int, int[]>[] freeVariableEquationIndexes, KeyValuePair<int, RationalNumber>[] freeVariableValues, KeyValuePair<int, RationalNumber>[] constantVariableValues, NumberRange<long>[] variableValueLimits)
    {
        if (freeVariableValues.Length == freeVariableEquationIndexes.Length)
        {
            var variableValues = new RationalNumber[equations.Count];

            foreach (var constantVariableValue in constantVariableValues)
            {
                variableValues[constantVariableValue.Key] = constantVariableValue.Value;
            }

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

        var currentVariable = freeVariableEquationIndexes[freeVariableValues.Length].Key;

        var inequalities = new List<LinearInequality>();

        foreach (var equationIndex in freeVariableEquationIndexes[freeVariableValues.Length].Value)
        {
            var equation = equations[equationIndex];

            var modifiedExpression = equation.Expression.Clone();
            foreach (var freeVariableValue in freeVariableValues)
            {
                modifiedExpression.SetVariableValue(freeVariableValue.Key, freeVariableValue.Value);
            }

            var limits = variableValueLimits[equationIndex];
            inequalities.Add(new LinearInequality(modifiedExpression, currentVariable, LinearInequalityConstraint.GreaterThanOrEqual, new RationalNumber(limits.Start)));
            inequalities.Add(new LinearInequality(modifiedExpression, currentVariable, LinearInequalityConstraint.LessThanOrEqual, new RationalNumber(limits.End)));
        }

        var lowerBound = long.MinValue;
        var upperBound = long.MaxValue;

        foreach (var inequality in inequalities)
        {
            if (inequality.Expression.IsConstantExpression())
            {
                var constant = inequality.Expression.GetConstant();

                if (inequality.Constraint == LinearInequalityConstraint.GreaterThanOrEqual)
                {
                    lowerBound = Math.Max(lowerBound, constant.Ceiling().LongValue);
                }
                else if (inequality.Constraint == LinearInequalityConstraint.LessThanOrEqual)
                {
                    upperBound = Math.Min(upperBound, constant.Floor().LongValue);
                }
            }
        }

        for (var value = lowerBound; value <= upperBound; value++)
        {
            KeyValuePair<int, RationalNumber>[] nextFreeVariableValues = [.. freeVariableValues, new KeyValuePair<int, RationalNumber>(currentVariable, new RationalNumber(value))];

            foreach (var solution in FindSolutions(equations, freeVariableEquationIndexes, nextFreeVariableValues, constantVariableValues, variableValueLimits))
            {
                yield return solution;
            }
        }
    }
}
