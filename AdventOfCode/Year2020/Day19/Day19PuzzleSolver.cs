using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2020.Day19;

[Puzzle(2020, 19, "Monster Messages")]
public class Day19PuzzleSolver : IPuzzleSolver
{
    private List<Rule> _rules = [];
    private string[] _messages = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        var knownRules = new Dictionary<int, Rule>();
        foreach (var ruleLine in chunks[0])
        {
            var splits1 = ruleLine.Split(": ");
            var ruleId = int.Parse(splits1[0]);

            if (!knownRules.TryGetValue(ruleId, out var currentRule))
            {
                currentRule = new Rule(ruleId);
                knownRules[ruleId] = currentRule;
            }

            if (splits1[1].Contains('"'))
            {
                currentRule.MakeItCharacterRule(splits1[1][1]);
            }
            else if (splits1[1].Contains('|'))
            {
                var orRuleChildren = new List<Rule>();
                var splits2 = splits1[1].Split(" | ");

                foreach(var andRuleSplits in splits2)
                {
                    var andRuleChildren = new List<Rule>();

                    foreach (var andRuleId in andRuleSplits.Split(' ').Select(int.Parse))
                    {
                        if (!knownRules.TryGetValue(andRuleId, out var andRuleChild))
                        {
                            andRuleChild = new Rule(andRuleId);
                            knownRules[andRuleId] = andRuleChild;
                        }

                        andRuleChildren.Add(andRuleChild);
                    }

                    var andRule = new Rule(-1); // Not a real rule id
                    andRule.MakeItAndRule(andRuleChildren);
                    orRuleChildren.Add(andRule);
                }

                currentRule.MakeItOrRule(orRuleChildren);
            }
            else
            {
                var andRuleChildren = new List<Rule>();

                foreach (var andRuleId in splits1[1].Split(' ').Select(int.Parse))
                {
                    if (!knownRules.TryGetValue(andRuleId, out var andRuleChild))
                    {
                        andRuleChild = new Rule(andRuleId);
                        knownRules[andRuleId] = andRuleChild;
                    }

                    andRuleChildren.Add(andRuleChild);
                }

                currentRule.MakeItAndRule(andRuleChildren);
            }
        }

        _rules = [.. knownRules.Values];
        _messages = chunks[1];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var rule0 = _rules.First(rule => rule.Id == 0);

        var answer = _messages.Count(rule0.Matches);

        return new PuzzleAnswer(answer, 139);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var interestingRuleIds = new int[]
        {
            0, 8, 42, 11, 31
        };

        var interestingRules = _rules.Where(rule => interestingRuleIds.Contains(rule.Id))
            .ToList();

        var rule0 = interestingRules.First(rule => rule.Id == 0);
        var rule8 = interestingRules.First(rule => rule.Id == 8);
        var rule42 = interestingRules.First(rule => rule.Id == 42);
        var rule11 = interestingRules.First(rule => rule.Id == 11);
        var rule31 = interestingRules.First(rule => rule.Id == 31);

        // Rule 8
        var rule8SecondRule = new Rule(-1);
        rule8SecondRule.MakeItAndRule([rule42, rule8]);

        rule8.MakeItOrRule([rule42, rule8SecondRule]);

        // Rule 11
        var rule11FirstRule = new Rule(-1);
        rule11FirstRule.MakeItAndRule([rule42, rule31]);

        var rule11SecondRule = new Rule(-1);
        rule11SecondRule.MakeItAndRule([rule42, rule11, rule31]);

        rule11.MakeItOrRule([rule11FirstRule, rule11SecondRule]);


        var answer = _messages.Count(rule0.Matches);

        return new PuzzleAnswer(answer, 289);
    }
}