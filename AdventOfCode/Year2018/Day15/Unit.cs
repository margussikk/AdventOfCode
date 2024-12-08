using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2018.Day15;
internal class Unit
{
    public UnitType UnitType {  get; set; }

    public GridCoordinate Coordinate { get; set; }

    public int HitPoints { get ; set; }

    public int AttackPower { get; set; }

    public bool Acted { get; set; }
}
