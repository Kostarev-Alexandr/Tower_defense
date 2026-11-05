namespace TowerDefense.Model;

// Хранит положение врага и перемещает его по маршруту в координатах клеток.
public class Enemy
{
    private readonly IReadOnlyList<GridPosition> route;
    private double distanceTravelled;

    public double Column { get; private set; }
    public double Row { get; private set; }
    public double Speed { get; } = 2;
    public bool HasReachedBase => distanceTravelled >= route.Count - 1;

    public Enemy(GameMap map)
    {
        route = map.Route;
        Column = map.Spawn.Column;
        Row = map.Spawn.Row;
    }

    public void Move(double seconds)
    {
        if (!double.IsFinite(seconds) || seconds < 0)
            throw new ArgumentOutOfRangeException(nameof(seconds));

        distanceTravelled = Math.Min(distanceTravelled + Speed * seconds, route.Count - 1);
        int index = (int)distanceTravelled;

        if (HasReachedBase)
        {
            Column = route[index].Column;
            Row = route[index].Row;
            return;
        }

        double progress = distanceTravelled - index;
        GridPosition current = route[index];
        GridPosition next = route[index + 1];
        Column = current.Column + (next.Column - current.Column) * progress;
        Row = current.Row + (next.Row - current.Row) * progress;
    }
}
