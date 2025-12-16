using AdventOfCode.Utilities.Numerics;
using System.Text;

namespace AdventOfCode.Utilities.Mathematics;

internal class LinearExpression
{
    public List<LinearTerm> Terms { get; private set; } = [];

    public LinearExpression(List<LinearTerm> terms)
    {
        SetLinearTerms(terms);
    }

    public bool ContainsVariable(int variable)
    {
        return Terms.Any(x => x.Variable == variable);
    }

    public bool IsConstantExpression()
    {
        return Terms.Count == 1 && Terms[0].Variable == null;
    }

    public RationalNumber GetConstant()
    {
        if (!IsConstantExpression())
        {
            throw new InvalidOperationException("Expression is not a constant expression");
        }

        return Terms[0].Coefficient;
    }

    public LinearExpression Clone()
    {
        return new LinearExpression([.. Terms.Select(x => x.Clone())]);
    }

    public RationalNumber Evaluate(RationalNumber[] variables)
    {
        var result = RationalNumber.Zero;

        foreach (var term in Terms)
        {
            if (term.Variable.HasValue)
            {
                result += term.Coefficient * variables[term.Variable.Value];
            }
            else
            {
                result += term.Coefficient;
            }
        }

        return result;
    }

    public void Negate()
    {
        for (var i = 0; i < Terms.Count; i++)
        {
            Terms[i] = Terms[i].Negated();
        }
    }

    public LinearTerm ExtractVariable(int variable)
    {
        var term = Terms.FirstOrDefault(x => x.Variable == variable)
            ?? throw new InvalidOperationException($"Variable x{variable} not found");

        Terms.Remove(term);

        return term;
    }

    public void AddConstant(RationalNumber value)
    {
        Terms.Add(new LinearTerm
        {
            Coefficient = value,
        });

        SetLinearTerms(Terms);
    }

    public void Divide(RationalNumber divisor)
    {
        for (var i = 0; i < Terms.Count; i++)
        {
            Terms[i] = Terms[i].DividedBy(divisor);
        }
    }

    public void SetVariableValue(int variable, RationalNumber value)
    {
        var optimize = false;

        for (var i = 0; i < Terms.Count; i++)
        {
            if (Terms[i].Variable == variable)
            {
                Terms[i] = new LinearTerm
                {
                    Coefficient = Terms[i].Coefficient * value,
                    Variable = null
                };

                optimize = true;
            }
        }

        if (optimize)
        {
            SetLinearTerms(Terms);
        }
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        for (var index = 0; index < Terms.Count; index++)
        {
            var linearTerm = Terms[index];
            if (linearTerm.Coefficient > RationalNumber.Zero && index != 0)
            {
                stringBuilder.Append('+');
            }

            stringBuilder.Append(linearTerm);
        }

        return stringBuilder.ToString();
    }

    private void SetLinearTerms(List<LinearTerm> linearTerms)
    {
        Terms = [.. linearTerms
            .GroupBy(x => x.Variable)
            .Select(x => new LinearTerm
            {
                Coefficient = x.Aggregate(RationalNumber.Zero, (agg, curr) => agg + curr.Coefficient),
                Variable = x.Key
            })
            .Where(x => x.Variable == null || x.Coefficient != RationalNumber.Zero)];
    }
}
