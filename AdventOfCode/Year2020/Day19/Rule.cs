namespace AdventOfCode.Year2020.Day19;

internal class Rule(int id)
{
    public RuleType RuleType { get; private set; } = RuleType.Unknown;

    public char Character { get; private set; } = '?';

    public List<Rule> AndRules { get; private set; } = [];

    public List<Rule> OrRules { get; private set; } = [];

    public int Id { get; } = id;

    public void MakeItCharacterRule(char character)
    {
        RuleType = RuleType.Character;
        Character = character;
    }

    public void MakeItAndRule(List<Rule> rules)
    {
        RuleType = RuleType.And;
        AndRules = rules;
    }

    public void MakeItOrRule(List<Rule> rules)
    {
        RuleType = RuleType.Or;
        OrRules = rules;
    }

    public bool Matches(string message)
    {
        var result = Matches(this, 0, message, 0);

        return result.Any(r => r.NextIndex == message.Length);

        // Local method
        static IEnumerable<(bool Matches, int NextIndex)> Matches(Rule currentRule, int ruleDepth, string message, int currentIndex)
        {
            if (ruleDepth > message.Length + 1) // Magic 1
            {
                // Do not continue
            }
            else if (currentIndex >= message.Length)
            {
                // Do not continue
            }
            else if (currentRule.RuleType == RuleType.Character)
            {
                if (message[currentIndex] == currentRule.Character)
                {
                    yield return (true, currentIndex + 1);
                }
            }
            else if (currentRule.RuleType == RuleType.And)
            {
                List<int> currentIndexes = [currentIndex];

                foreach (var rule in currentRule.AndRules)
                {
                    ruleDepth++;

                    var nextIndexes = new List<int>();

                    foreach (var index in currentIndexes)
                    {
                        var results = Matches(rule, ruleDepth, message, index);
                        foreach (var (matches, nextIndex) in results)
                        {
                            if (matches)
                            {
                                nextIndexes.Add(nextIndex);
                            }
                        }
                    }

                    currentIndexes = nextIndexes;
                }

                foreach (var index in currentIndexes)
                {
                    yield return (true, index);
                }
            }
            else // Or
            {
                foreach (var rule in currentRule.OrRules)
                {
                    var results = Matches(rule, ruleDepth + 1, message, currentIndex);
                    foreach (var result in results)
                    {
                        if (result.Matches)
                        {
                            yield return (result.Matches, result.NextIndex);
                        }
                    }
                }
            }
        }
    }
}
