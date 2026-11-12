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

    internal bool TryAttack(Enemy enemy, double seconds)
    {
        cooldown = Math.Max(0, cooldown - seconds);
        double columnDistance = enemy.Column - Position.Column;
        double rowDistance = enemy.Row - Position.Row;
        double distanceSquared = columnDistance * columnDistance + rowDistance * rowDistance;

        if (cooldown > 1e-9 || distanceSquared > AttackRange * AttackRange)
            return false;

        enemy.TakeDamage(Damage);
        cooldown = ShotInterval;
        return true;
    }
}
