namespace AdventOfCode.Year2021.Day18;

internal class SnailfishNumber
{
    public int? Number { get; set; }
    public SnailfishNumber? Parent { get; set; }
    public SnailfishNumber? Left { get; set; }
    public SnailfishNumber? Right { get; set; }

    public int Level => Parent != null ? Parent.Level + 1 : 0;

    public SnailfishNumber AddAndReduce(SnailfishNumber snailfishNumber)
    {
        var newSnailfishNumber = new SnailfishNumber
        {
            Left = this,
            Right = snailfishNumber
        };

        newSnailfishNumber.Left.Parent = newSnailfishNumber;
        newSnailfishNumber.Right.Parent = newSnailfishNumber;

        newSnailfishNumber.Reduce();

        return newSnailfishNumber;
    }

    public void Reduce()
    {
        while (Explode() || Split())
        {
            // Keep reducing
        }
    }

    public int Magnitude()
    {
        if (Number.HasValue)
        {
            return Number.Value;
        }
        else if (Left != null && Right != null)
        {
            return Left.Magnitude() * 3 + Right.Magnitude() * 2;
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public bool Explode()
    {
        if (Level >= 4 && Left != null && Left.Number.HasValue && Right != null && Right.Number.HasValue)
        {
            // Explode Left
            // Pair's left value is added to the first regular number to the left of the exploding pair (if any)
            var child = this;
            var parent = Parent;
            var explodedLeft = false;

            while (!explodedLeft && parent != null)
            {
                if (parent.Left == child)
                {
                    // Ignore
                    child = parent;
                    parent = parent.Parent;
                    continue;
                }

                explodedLeft = parent.ExplodeLeft(true, Left.Number.Value);
            }

            // Explode Right
            // Pair's right value is added to the first regular number to the right of the exploding pair (if any)
            child = this;
            parent = Parent;
            var explodedRight = false;

            while (!explodedRight && parent != null)
            {
                if (parent.Right == child)
                {
                    // Ignore
                    child = parent;
                    parent = parent.Parent;
                    continue;
                }

                explodedRight = parent.ExplodeRight(true, Right.Number.Value);
            }

            Left = null;
            Right = null;
            Number = 0;

            return true;
        }

        if (Left != null && Left.Explode())
        {
            return true;
        }

        if (Right != null && Right.Explode())
        {
            return true;
        }

        return false;
    }
    public bool ExplodeLeft(bool skipRight, int value)
    {
        if (Number.HasValue)
        {
            Number += value;
            return true;
        }

        if (!skipRight && Right != null && Right.ExplodeLeft(false, value))
        {
            return true;
        }

        if (Left != null && Left.ExplodeLeft(false, value))
        {
            return true;
        }

        return false;
    }

    public bool ExplodeRight(bool skipLeft, int value)
    {
        if (Number.HasValue)
        {
            Number += value;
            return true;
        }

        if (!skipLeft && Left != null && Left.ExplodeRight(false, value))
        {
            return true;
        }

        if (Right != null && Right.ExplodeRight(false, value))
        {
            return true;
        }

        return false;
    }

    public bool Split()
    {
        if (Number.HasValue && Number >= 10)
        {
            Left = new SnailfishNumber
            {
                Parent = this,
                Number = Number / 2
            };

            Right = new SnailfishNumber
            {
                Parent = this,
                Number = Number / 2 + (Number % 2)
            };

            Number = null;

            return true;
        }

        if (Left != null && Left.Split())
        {
            return true;
        }

        if (Right != null && Right.Split())
        {
            return true;
        }

        return false;
    }

    public override string ToString()
    {
        if (Number.HasValue)
        {
            return $"{Number}";
        }
        else
        {
            return $"[{Left},{Right}]";
        }
    }

    public SnailfishNumber DeepCopy(SnailfishNumber? parent = null)
    {
        var newSnailfishNumber = new SnailfishNumber
        {
            Parent = parent,
            Number = Number
        };

        if (Left != null)
        {
            newSnailfishNumber.Left = Left.DeepCopy(newSnailfishNumber);
        }

        if (Right != null)
        {
            newSnailfishNumber.Right = Right.DeepCopy(newSnailfishNumber);
        }

        return newSnailfishNumber;
    }

    public static SnailfishNumber Parse(string line)
    {
        int index = 0;

        return ParseLocal(ref index);

        SnailfishNumber ParseLocal(ref int indexRef, SnailfishNumber? parent = null)
        {
            var snailfishNumber = new SnailfishNumber
            {
                Parent = parent
            };

            if (char.IsDigit(line[indexRef]))
            {
                var startIndex = indexRef;

                while (char.IsDigit(line[indexRef]))
                {
                    indexRef++;
                }

                snailfishNumber.Number = int.Parse(line[startIndex..indexRef]);
                return snailfishNumber;
            }

            if (line[indexRef] == '[')
            {
                indexRef++;
                snailfishNumber.Left = ParseLocal(ref indexRef, snailfishNumber);
            }

            if (line[indexRef] == ',')
            {
                indexRef++;
                snailfishNumber.Right = ParseLocal(ref indexRef, snailfishNumber);
            }

            if (line[index] == ']')
            {
                indexRef++;
                return snailfishNumber;
            }

            throw new InvalidOperationException();
        }
    }
}
