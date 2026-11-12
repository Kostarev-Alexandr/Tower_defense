namespace TowerDefense.Model;

// Хранит точки выстрела, чтобы его было видно после нанесения урона.
public class Shot
{
    public GridPosition Origin { get; }
    public double TargetColumn { get; }
    public double TargetRow { get; }
    internal double TimeRemaining { get; set; } = 0.13;

    internal Shot(GridPosition origin, Enemy enemy)
    {
        Origin = origin;
        TargetColumn = enemy.Column;
        TargetRow = enemy.Row;
    }
}
