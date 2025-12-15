using AdventOfCode.Utilities.Numerics;
using System.Text;

namespace AdventOfCode.Utilities.Mathematics;

internal class LinearTerm
{
    public RationalNumber Coefficient { get; init; }
    public int? Variable { get; init; }

    public LinearTerm Clone()
    {
        return new LinearTerm
        {
            Coefficient = Coefficient,
            Variable = Variable
        };
    }

    public LinearTerm Negated()
    {
        return new LinearTerm
        {
            Coefficient = RationalNumber.Zero - Coefficient,
            Variable = Variable
        };
    }

    public LinearTerm DividedBy(RationalNumber divisor)
    {
        return new LinearTerm
        {
            Coefficient = Coefficient / divisor,
            Variable = Variable
        };
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        if (!Variable.HasValue)
        {
            stringBuilder.Append(Coefficient);
        }
        else
        {
            if (Coefficient == new RationalNumber(-1))
            {
                stringBuilder.Append('-');
            }
            else if (Coefficient != RationalNumber.One)
            {
                stringBuilder.Append($"{Coefficient}");
            }

            stringBuilder.Append($"x{Variable}");
        }

        return stringBuilder.ToString();
    }
}
