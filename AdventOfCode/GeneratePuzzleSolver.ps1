if ($args.count -ne 2) {
	Write-Host "Usage: GeneratePuzzleSolver.ps1 <year> <day>"
	Exit
}

$year = $args[0]
$day = $args[1]

$template = @"
using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year<YEAR>.Day<DAYXX>;

[Puzzle(<YEAR>, <DAYX>, `"`")]
public class Day<DAYXX>PuzzleSolver : IPuzzleSolver
{
    public void ParseInput(string[] inputLines)
    {
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        return new PuzzleAnswer(`"TODO`", `"TODO`");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        return new PuzzleAnswer(`"TODO`", `"TODO`");
    }
}
"@

$newDirectory = Join-Path "Year$Year" "Day$("{0:00}" -f $day)"

if(!(Test-Path $newDirectory)) {
    New-Item $newDirectory -ItemType Directory | Out-Null
}

$newFile = Join-Path $newDirectory "Day$("{0:00}" -f $day)PuzzleSolver.cs"  
if(!(Test-Path $newFile)) {
	New-Item $newFile -ItemType File -Value ($template -replace "<YEAR>", $Year -replace "<DAYXX>", "$("{0:00}" -f $day)" -replace "<DAYX>", $day) -Force | Out-Null
}

Write-Host "$newFile created"