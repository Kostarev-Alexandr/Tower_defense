namespace TowerDefense.Model;

public class GameMap
{
    private readonly CellType[,] cells;

    public int Width { get; } = 12;
    public int Height { get; } = 8;

    // Порядок точек задаёт направление движения: от входа к базе.
    public IReadOnlyList<GridPosition> Route { get; }
    public GridPosition Spawn => Route[0];
    public GridPosition Base => Route[Route.Count - 1];

    public GameMap()
    {
        cells = new CellType[Width, Height];

        for (int column = 0; column < Width; column++)
        {
            for (int row = 0; row < Height; row++)
            {
                cells[column, row] = CellType.Grass;
            }
        }

        GridPosition[] route =
        {
            new GridPosition(0, 3),
            new GridPosition(1, 3),
            new GridPosition(2, 3),
            new GridPosition(3, 3),
            new GridPosition(3, 2),
            new GridPosition(3, 1),
            new GridPosition(4, 1),
            new GridPosition(5, 1),
            new GridPosition(6, 1),
            new GridPosition(7, 1),
            new GridPosition(7, 2),
            new GridPosition(7, 3),
            new GridPosition(7, 4),
            new GridPosition(7, 5),
            new GridPosition(8, 5),
            new GridPosition(9, 5),
            new GridPosition(10, 5),
            new GridPosition(11, 5)
        };

        Route = Array.AsReadOnly(route);

        foreach (GridPosition position in Route)
        {
            cells[position.Column, position.Row] = CellType.Road;
        }

        cells[Spawn.Column, Spawn.Row] = CellType.Spawn;
        cells[Base.Column, Base.Row] = CellType.Base;
    }

    public bool IsInside(int column, int row)
    {
        return column >= 0 && column < Width && row >= 0 && row < Height;
    }

    public CellType GetCellType(int column, int row)
    {
        if (column < 0 || column >= Width)
            throw new ArgumentOutOfRangeException(nameof(column));

        if (row < 0 || row >= Height)
            throw new ArgumentOutOfRangeException(nameof(row));

        return cells[column, row];
    }

    public bool IsBuildableCell(int column, int row)
    {
        return IsInside(column, row) && cells[column, row] == CellType.Grass;
    }
}
