using System.Text;

namespace AdventOfCode.Year2020.Day18;

internal class Expression
{
    public List<IElement> Elements { get; } = [];

    public long Evaluate(Func<OperatorType, OperatorType, bool> precedence)
    {
        var operandValuesStack = new Stack<long>();
        var operatorAndParenthesesStack = new Stack<IElement>();

        foreach (var element in Elements)
        {
            switch (element)
            {
                case OperandElement operandElement:
                    operandValuesStack.Push(operandElement.Value);
                    break;
                case ParenthesesElement { Open: true } parenthesesElement:
                    operatorAndParenthesesStack.Push(parenthesesElement);
                    break;
                // Close parentheses
                case ParenthesesElement:
                {
                    ProcessStacks(operandValuesStack, operatorAndParenthesesStack);

                    if (operatorAndParenthesesStack.Count > 0 &&
                        operatorAndParenthesesStack.Peek() is ParenthesesElement { Open: true })
                    {
                        operatorAndParenthesesStack.Pop();
                    }

                    break;
                }
                case OperatorElement operatorElement:
                    ProcessStacks(operandValuesStack, operatorAndParenthesesStack, previousOperatorType => precedence(previousOperatorType, operatorElement.OperatorType));

                    operatorAndParenthesesStack.Push(operatorElement);
                    break;
            }
        }

        // Process stacks
        ProcessStacks(operandValuesStack, operatorAndParenthesesStack);

        return operandValuesStack.Pop();
    }

    public static Expression Parse (string input)
    {
        var expression = new Expression();

        var span = input.AsSpan();
        while(span.Length > 0)
        {
            switch (span[0])
            {
                // Ignore spaces
                case ' ':
                    span = span[1..];
                    break;
                case '(':
                    expression.Elements.Add(new ParenthesesElement(true));
                    span = span[1..];
                    break;
                case ')':
                    expression.Elements.Add(new ParenthesesElement(false));
                    span = span[1..];
                    break;
                case '+':
                    expression.Elements.Add(new OperatorElement(OperatorType.Addition));
                    span = span[1..];
                    break;
                case '*':
                    expression.Elements.Add(new OperatorElement(OperatorType.Multiplication));
                    span = span[1..];
                    break;
                default:
                {
                    if (char.IsDigit(span[0]))
                    {
                        var index = span.IndexOfAnyExceptInRange('0', '9');
                        if (index < 0)
                        {
                            index = span.Length;
                        }

                        var value = long.Parse(span[..index]);
                        expression.Elements.Add(new OperandElement(value));
                        span = span[index..];
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid expression character");
                    }

                    break;
                }
            }
        }

        return expression;
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        foreach (var element in Elements)
        {
            if (element is OperatorElement)
            {
                stringBuilder.Append($" {element} ");
            }
            else
            {
                stringBuilder.Append(element);
            }
        }

        return stringBuilder.ToString();
    }

    private static void ProcessStacks(Stack<long> operandValuesStack, Stack<IElement> operatorAndParenthesesStack, Func<OperatorType, bool>? precedencePredicate = null)
    {
        while (operatorAndParenthesesStack.Count > 0 &&
               operatorAndParenthesesStack.Peek() is OperatorElement previousOperatorElement &&
               (precedencePredicate?.Invoke(previousOperatorElement.OperatorType) ?? true))
        {
            previousOperatorElement = (OperatorElement)operatorAndParenthesesStack.Pop();

            var value2 = operandValuesStack.Pop();
            var value1 = operandValuesStack.Pop();

            var result = previousOperatorElement.Calculate(value1, value2);
            operandValuesStack.Push(result);
        }
    }
}
