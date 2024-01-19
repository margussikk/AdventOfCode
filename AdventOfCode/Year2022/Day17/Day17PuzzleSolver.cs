using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2022.Day17;

[Puzzle(2022, 17, "Pyroclastic Flow")]
public class Day17PuzzleSolver : IPuzzleSolver
{
    private List<Rock> _rocks = [];
    private List<PushDirection> _pushDirections = [];

    public void ParseInput(string[] inputLines)
    {
        _rocks =
        [
            Rock.Parse("####"),

            Rock.Parse(".#.",
                       "###",
                       ".#."),

            Rock.Parse("..#",
                       "..#",
                       "###"),

            Rock.Parse("#",
                       "#",
                       "#",
                       "#"),

            Rock.Parse("##",
                       "##"),
        ];

        _pushDirections = inputLines[0]
            .Select(x => x == '<' ? PushDirection.Left : PushDirection.Right)
            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(2022);

        return new PuzzleAnswer(answer, 3127);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(1_000_000_000_000L);

        return new PuzzleAnswer(answer, 1542941176480L);
    }

    private long GetAnswer(long rockCount)
    {
        var chamber = new Chamber();

        var directionIndex = 0;

        var states = new Dictionary<(int stoneIndex, int directionIndex, int hashCode), (long Index, int TowerHeight)>();

        var towerHeightsHistory = new List<int>();

        for (var index = 0L; index < rockCount; index++)
        {
            // Rock appears
            var rockIndex = Convert.ToInt32(index % _rocks.Count);
            var rock = _rocks[rockIndex];

            chamber.AppearRock(rock);

            var currentTowerHeight = chamber.GetTowerHeight();
            var hashCode = chamber.CalculateHashCode();

            var key = (rockIndex, directionIndex, hashCode);
            if (states.TryGetValue(key, out var result))
            {
                var stillToGo = rockCount - index;
                var cycleLength = index - result.Index;

                var cyclesLeft = stillToGo / cycleLength;
                var cycleTowerHeight = currentTowerHeight - result.TowerHeight;
                var heightFromCycles = cyclesLeft * cycleTowerHeight;

                var indexInCycle = stillToGo % cycleLength;
                var naturalHeight = currentTowerHeight + towerHeightsHistory[(int)(result.Index + indexInCycle)] - result.TowerHeight;

                return naturalHeight + heightFromCycles;
            }
            else
            {
                states[key] = (index, currentTowerHeight);
                towerHeightsHistory.Add(currentTowerHeight);
            }

            bool canMoveDown;
            do
            {
                var direction = _pushDirections[directionIndex];
                directionIndex = (directionIndex + 1) % _pushDirections.Count;

                // Jet pushes right or left
                if (direction == PushDirection.Right && chamber.CanMoveRock(rock, 0, 1))
                {
                    rock.Column++;
                }
                else if (direction == PushDirection.Left && chamber.CanMoveRock(rock, 0, -1))
                {
                    rock.Column--;
                }

                // Rock falls one unit
                canMoveDown = chamber.CanMoveRock(rock, -1, 0);
                if (canMoveDown)
                {
                    rock.Row--;
                }
            } while (canMoveDown);

            chamber.RestRock(rock);
        }

        return 0;
    }
}