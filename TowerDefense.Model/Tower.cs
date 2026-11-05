namespace TowerDefense.Model;

public class Tower
{
    public GridPosition Position { get; }

    public Tower(GridPosition position)
    {
        Position = position;
    }
}
