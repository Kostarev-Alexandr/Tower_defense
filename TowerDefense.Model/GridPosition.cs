namespace TowerDefense.Model;

// Координаты в клетках
public class GridPosition
{
    public int Column { get; }
    public int Row { get; }

    public GridPosition(int column, int row)
    {
        Column = column;
        Row = row;
    }
}
