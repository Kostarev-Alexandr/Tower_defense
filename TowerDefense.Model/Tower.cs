namespace TowerDefense.Model;

// Хранит параметры атаки и время до следующего выстрела.
public class Tower
{
    public const double AttackRange = 2.5;
    public const int Damage = 20;
    public const double ShotInterval = 0.65;

    private double cooldown;

    public GridPosition Position { get; }

    public Tower(GridPosition position)
    {
        Position = position;
    }

    internal void ResetCooldown()
    {
        cooldown = 0;
    }

    // Враги хранятся в порядке появления, поэтому первой выбирается передняя цель.
    internal Enemy? Attack(IReadOnlyList<Enemy> enemies, double seconds)
    {
        cooldown = Math.Max(0, cooldown - seconds);
        if (cooldown > 1e-9)
            return null;

        foreach (Enemy enemy in enemies)
        {
            double columnDistance = enemy.Column - Position.Column;
            double rowDistance = enemy.Row - Position.Row;
            double distanceSquared = columnDistance * columnDistance + rowDistance * rowDistance;
            if (distanceSquared > AttackRange * AttackRange)
                continue;

            enemy.TakeDamage(Damage);
            cooldown = ShotInterval;
            return enemy;
        }

        return null;
    }
}
